using System.Text;
using System.Text.Json;
using Godot;

namespace NewGameProject.LoginUI;

public partial class LoginUIScreen : Control
{
	// ---------- runtime refs: holds references to UI controls and HTTP request node ----------
	private LineEdit _user, _email, _pass;
	private RichTextLabel _status;
	private AcceptDialog _popupDialog;
	private Button _playButton;		// button to start game after login
	private string _jwt = "";		// stores the JWT token after login
	private string _currentAction = "";	// tracks which API call is in flight

	private HttpRequest _http;		// sends http requests to backend
	private const string BASE = "http://localhost:5011"; // base URL of api

	// ---------- startup: runs once the scene loads ----------
	public override void _Ready()
	{
		GD.Print("LoginUI READY");
		// Grab UI nodes from the scene tree
		_user   = GetNode<LineEdit>("CenterContainer/VBoxContainer/UsernameField");
		_email  = GetNode<LineEdit>("CenterContainer/VBoxContainer/EmailField");
		_pass   = GetNode<LineEdit>("CenterContainer/VBoxContainer/PasswordField");
		_status = GetNode<RichTextLabel>("CenterContainer/VBoxContainer/StatusLabel");
		_popupDialog = GetNode<AcceptDialog>("PopupDialog");
		_playButton = GetNode<Button>("CenterContainer/VBoxContainer/PlayButton");
		
		// Disables play button until user logs in
		_playButton.Disabled = true;
		_playButton.Pressed += OnPlayPressed;

		// Wires up button signals for API calls
		GetNode<Button>("CenterContainer/VBoxContainer/RegisterButton").Pressed    += OnRegister;
		GetNode<Button>("CenterContainer/VBoxContainer/LoginButton").Pressed       += OnLogin;
		GetNode<Button>("CenterContainer/VBoxContainer/LeaderBoardButton").Pressed += OnLeaderboard;

		// Create and configure the HTTPRequest node
		_http = new HttpRequest();
		AddChild(_http);
		_http.RequestCompleted += OnRequestDone;
	}

	// ---------- Register button ----------
	private void OnRegister()
	{
		// Prepare JSON from input fields
		var json = JsonSerializer.Serialize(new
		{
			Username = _user.Text,
			Email    = _email.Text,
			Password = _pass.Text
		});
		_currentAction = "register";
		_http.Request(
			BASE + "/api/v1/auth/register",
			new[] { "Content-Type: application/json" },
			HttpClient.Method.Post,
			json);
		_status.Text = "Registering…";
	}

	// Login button
	private void OnLogin()
	{
		// Sends login credentials
		var json = JsonSerializer.Serialize(new
		{
			username = _user.Text,
			Password = _pass.Text
		});
		_currentAction = "login";
		_http.Request(
			BASE + "/api/v1/auth/login",
			new[] { "Content-Type: application/json" },
			HttpClient.Method.Post,
			json);
		_status.Text = "Logging in…";
	}

	// Leaderboard button
	private void OnLeaderboard()
	{
		string[] headers = _jwt == "" ? null : new[] { $"Authorization: Bearer {_jwt}" };
		_currentAction = "leaderboard";
		_http.Request(BASE + "/api/v1/leaderboard", headers);
		_status.Text = "Loading leaderboard…";
	}

	// ---------- HTTP response handler ----------
	private void OnRequestDone(long result, long code, string[] _h, byte[] body)
	{
		var text = body.Length > 0 ? Encoding.UTF8.GetString(body) : "";

		// show errors immediately
		if (code < 200 || code >= 300)
		{
			_status.Text = $"Error {code}: {text}";
			return;
		}

		// handles success for each action type
		switch (_currentAction)
		{
			case "register":
				_popupDialog.DialogText = "Registered successfully!";
				_popupDialog.Popup();
				break;

			case "login":
			{
				using var doc = JsonDocument.Parse(text);
				if (doc.RootElement.TryGetProperty("token", out var tok) &&
				    tok.ValueKind == JsonValueKind.String)
				{
					_jwt = tok.GetString();
					
					_playButton.Disabled = false;
					
					_popupDialog.DialogText = "Logged in successfully!";
					_popupDialog.Popup();
				}
				else
				{
					_popupDialog.DialogText = "Login succeeded but token missing";
					_popupDialog.Popup();
				}
				break;
			}

			case "leaderboard":
				_status.Text = text;      // raw JSON; keep it simple
				break;
		}

		_currentAction = "";
	}
	// starts the game scene
	private void OnPlayPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn");
	}
	
}