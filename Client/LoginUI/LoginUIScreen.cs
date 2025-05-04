using Godot;
using System.Text;
using System.Text.Json;

public partial class LoginUIScreen : Control
{
	// ---------- runtime refs ----------
	private LineEdit _user, _email, _pass;
	private RichTextLabel _status;
	private AcceptDialog _popupDialog;
	private string _jwt = "";
	private string _currentAction = "";

	private HttpRequest _http;
	private const string BASE = "http://localhost:5011";

	// ---------- startup ----------
	public override void _Ready()
	{
		GD.Print("LoginUI READY");
		// Grab UI nodes
		_user   = GetNode<LineEdit>("CenterContainer/VBoxContainer/UsernameField");
		_email  = GetNode<LineEdit>("CenterContainer/VBoxContainer/EmailField");
		_pass   = GetNode<LineEdit>("CenterContainer/VBoxContainer/PasswordField");
		_status = GetNode<RichTextLabel>("CenterContainer/VBoxContainer/StatusLabel");
		_popupDialog = GetNode<AcceptDialog>("PopupDialog");

		GetNode<Button>("CenterContainer/VBoxContainer/RegisterButton").Pressed    += OnRegister;
		GetNode<Button>("CenterContainer/VBoxContainer/LoginButton").Pressed       += OnLogin;
		GetNode<Button>("CenterContainer/VBoxContainer/LeaderBoardButton").Pressed += OnLeaderboard;

		// Add one HttpRequest node
		_http = new HttpRequest();
		AddChild(_http);
		_http.RequestCompleted += OnRequestDone;
	}

	// ---------- buttons ----------
	private void OnRegister()
	{
		var json = JsonSerializer.Serialize(new
		{
			Username = _user.Text,
			Email    = _email.Text,
			Password = _pass.Text
		});
		_currentAction = "register";
		_http.Request(
			BASE + "/api/auth/register",
			new[] { "Content-Type: application/json" },
			HttpClient.Method.Post,
			json);
		_status.Text = "Registering…";
	}

	private void OnLogin()
	{
		var json = JsonSerializer.Serialize(new
		{
			username = _user.Text,
			Password = _pass.Text
		});
		_currentAction = "login";
		_http.Request(
			BASE + "/api/auth/login",
			new[] { "Content-Type: application/json" },
			HttpClient.Method.Post,
			json);
		_status.Text = "Logging in…";
	}

	private void OnLeaderboard()
	{
		string[] headers = _jwt == "" ? null : new[] { $"Authorization: Bearer {_jwt}" };
		_currentAction = "leaderboard";
		_http.Request(BASE + "/api/leaderboard", headers);
		_status.Text = "Loading leaderboard…";
	}

	// ---------- HTTP callback ----------
	private void OnRequestDone(long result, long code, string[] _h, byte[] body)
	{
		var text = body.Length > 0 ? Encoding.UTF8.GetString(body) : "";

		if (code < 200 || code >= 300)
		{
			_status.Text = $"Error {code}: {text}";
			return;
		}

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
}
