using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class State : BaseUniqueObject<State>
{
    private static readonly int STATE_PERSON = 1;
    private static readonly int STATE_BUILDING = 2;
    public int state;

    private void Awake()
    {
        SetStateToPerson();
    }

    public bool IsPerson { get { return state == STATE_PERSON; } }
    public bool IsBuilding { get { return state == STATE_BUILDING; } }

    public void SetStateToPerson()
    {
        this.state = STATE_PERSON;
    }

    public void SetStateToBuilding()
    {
        this.state = STATE_BUILDING;
    }
}
