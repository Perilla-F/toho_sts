using System;

public interface ISaveScheduler
{
    void ScheduleSave(Action saveAction);
}