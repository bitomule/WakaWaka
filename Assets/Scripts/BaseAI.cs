using UnityEngine;
using System.Collections;
using System.Reflection;

public class BaseAI : MonoBehaviour {
	
	public delegate void StateFunc();
	
	public StateFunc stateAction;
	
	public float stateStartTime;
	public bool initVar;
	public float timer;
	
		
	float stateTime {
		get {
			return (Time.time - stateStartTime);
		}
	}
	
	public bool initialising {
		get {
			bool temp = initVar;
			initVar = false;
			return temp;
		}
	}
	
	//Nos devuelve un valor de 0 a 1 indicando cuanto tiempo del indicado en newtimer ha pasado.
	//Para cambiar de estado al pasar el tiempo usariamos if(TimeScale>1)
	public float TimeScale {
		get {
			return stateTime / timer;
		}
	}
	
	//El primer parámetro es la nueva función que queremos asignar al delegate, el segundo es el tiempo.
	public void ChangeState(StateFunc newState,float newtimer)
	{
			stateStartTime=Time.time;
			initVar=true;
			timer=newtimer;
			stateAction = newState;

	}
	
	
}
