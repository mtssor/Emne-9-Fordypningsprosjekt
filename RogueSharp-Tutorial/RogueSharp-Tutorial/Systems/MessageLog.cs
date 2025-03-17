using RLNET;

namespace RogueSharp_Tutorial.Systems;

public class MessageLog
{
    // define max nr of lines to store
    private static readonly int _maxLines = 9;
    
    // use a queue to keep track of the lines of text
    // the first line added to the log will also be the first removed
    private readonly Queue<string> _lines;

    public MessageLog()
    {
        _lines = new Queue<string>();
    }
    
    // add a line to the MessageLog queue
    public void Add(string message)
    {
        _lines.Enqueue(message);
        
        // when exceeding the maximum number of lines remove the oldest one
        if (_lines.Count > _maxLines)
        {
            _lines.Dequeue();
        }
    }
    
    // draw each line of the MessageLog queue to the console
    public void Draw(RLConsole console)
    {
        string[] lines = _lines.ToArray();
        for (int i = 0; i < lines.Length; i++)
        {
            console.Print(1, i + 1, lines[i], RLColor.White);
        }
    }
}