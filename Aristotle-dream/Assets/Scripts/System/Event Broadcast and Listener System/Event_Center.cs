using System;
using System.Collections.Generic;
using UnityEngine;

public class Event_Center
{
    #region Main_Part
    private static Dictionary<Event_Type, Delegate> event_table = new Dictionary<Event_Type, Delegate>();

    private static void On_Listener_Adding(Event_Type event_type , Delegate call_back)
    {
        if(!event_table.ContainsKey(event_type))
        {
            //If we dont't have it , add it.
            event_table.Add(event_type, call_back);
        }

        Delegate dele = event_table[event_type];
        if(dele!=null && dele.GetType()!=call_back.GetType())
        {
            throw new Exception(string.Format("Try another different delegate for {0} , presently neede type is {1} , what counterpart you give is {2}", event_type, dele.GetType(), call_back.GetType()));
        }
    }

    private static void On_Listener_Removing(Event_Type event_type , Delegate call_back)
    {
        if(event_table.ContainsKey(event_type))
        {
            Delegate dele = event_table[event_type];

            if(dele==null)
            {
                throw new Exception(string.Format("Reemove Listener wrong: event {0} doesn't have corresponding delegate", event_type));
            }
            if(dele.GetType() != call_back.GetType())
            {
                throw new Exception(string.Format("Remove Listener wrong: try to remove different type , presently is {1} , but you want to remove {2}" , dele.GetType() , call_back.GetType()));
            }

        }
        else
        {
            throw new Exception(string.Format("Remove Listener wrong: {0} doesn't exist", event_type));
        }
    }

    private static void On_Listener_Removed(Event_Type event_type)
    {
        if (event_table[event_type] == null)
        {
            event_table.Remove(event_type);
        }
    }
    #endregion

    #region Broadcast -- Number Zero
    public static void Add_Listener(Event_Type event_type , Call_Back call_back)
    {
        On_Listener_Adding(event_type, call_back);
        event_table[event_type] = (Call_Back)event_table[event_type] + call_back;
    }

    public static void Remove_Listener(Event_Type event_type , Call_Back call_back)
    {
        On_Listener_Removing(event_type, call_back);
        event_table[event_type] = (Call_Back)event_table[event_type] - call_back;
        On_Listener_Removed(event_type);
    }

    public static void Broad_Cast(Event_Type event_type)
    {
        Delegate dele;
       if(event_table.TryGetValue(event_type , out dele))
        {
            Call_Back call_back = dele as Call_Back;//If dele fail to convert , it'll be null.
            if (call_back != null) call_back();
            else throw new Exception(string.Format("Broadcast wrong: event {0} correspond different type", event_type)); 
        }
    }
    #endregion

    #region Broadcast -- Number One
    public static void Add_Listener<T>(Event_Type event_type, Call_Back<T> call_back)
    {
        On_Listener_Adding(event_type, call_back);
        event_table[event_type] = (Call_Back<T>)event_table[event_type] + call_back;
    }

    public static void Remove_Listener<T>(Event_Type event_type, Call_Back<T> call_back)
    {
        On_Listener_Removing(event_type, call_back);
        event_table[event_type] = (Call_Back<T>)event_table[event_type] - call_back;
        On_Listener_Removed(event_type);
    }

    public static void Broad_Cast<T>(Event_Type event_type , T arg)
    {
        Delegate dele;
        if (event_table.TryGetValue(event_type, out dele))
        {
            Call_Back<T> call_back = dele as Call_Back<T>;//If dele fail to convert , it'll be null.
            if (call_back != null) call_back(arg);
            else throw new Exception(string.Format("Broadcast wrong: event {0} correspond different type", event_type));
        }
    }
    #endregion

    #region Broadcast -- Number Two
    public static void Add_Listener<T,X>(Event_Type event_type, Call_Back<T, X> call_back)
    {
        On_Listener_Adding(event_type, call_back);
        event_table[event_type] = (Call_Back<T, X>)event_table[event_type] + call_back;
    }

    public static void Remove_Listener<T, X>(Event_Type event_type, Call_Back<T, X> call_back)
    {
        On_Listener_Removing(event_type, call_back);
        event_table[event_type] = (Call_Back<T, X>)event_table[event_type] - call_back;
        On_Listener_Removed(event_type);
    }

    public static void Broad_Cast<T, X>(Event_Type event_type, T arg1 , X arg2)
    {
        Delegate dele;
        if (event_table.TryGetValue(event_type, out dele))
        {
            Call_Back<T, X> call_back = dele as Call_Back<T, X>;//If dele fail to convert , it'll be null.
            if (call_back != null) call_back(arg1,arg2);
            else throw new Exception(string.Format("Broadcast wrong: event {0} correspond different type", event_type));
        }
    }
    #endregion

    #region Broadcast -- Number Three
    public static void Add_Listener<T, X, Y>(Event_Type event_type, Call_Back<T, X, Y> call_back)
    {
        On_Listener_Adding(event_type, call_back);
        event_table[event_type] = (Call_Back<T, X, Y>)event_table[event_type] + call_back;
    }

    public static void Remove_Listener<T, X, Y>(Event_Type event_type, Call_Back<T, X, Y> call_back)
    {
        On_Listener_Removing(event_type, call_back);
        event_table[event_type] = (Call_Back<T, X, Y>)event_table[event_type] - call_back;
        On_Listener_Removed(event_type);
    }

    public static void Broad_Cast<T, X, Y>(Event_Type event_type, T arg1, X arg2 , Y arg3)
    {
        Delegate dele;
        if (event_table.TryGetValue(event_type, out dele))
        {
            Call_Back<T, X, Y> call_back = dele as Call_Back<T, X, Y>;//If dele fail to convert , it'll be null.
            if (call_back != null) call_back(arg1, arg2, arg3);
            else throw new Exception(string.Format("Broadcast wrong: event {0} correspond different type", event_type));
        }
    }
    #endregion
}
