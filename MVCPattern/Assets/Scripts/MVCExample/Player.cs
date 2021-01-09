using System;

public class Player
{
    public delegate void PositionEvent(_Vector3 position);
    public event PositionEvent OnPositionChanged;
    
    public _Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            if(_position!=value)
            {
                _position = value;
                if(OnPositionChanged != null)
                {
                    OnPositionChanged(value);
                }
            }
        }
    }

    private _Vector3 _position;
}
