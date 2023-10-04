using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGoblin {
    public Dictionary<string, float> GetStats(); 
	public void Primary();
	public void Secondary();
}