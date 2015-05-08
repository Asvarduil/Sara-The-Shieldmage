using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueEventHook
{
    public string Name;
    public Func<List<string>, IEnumerator> Function;
}

