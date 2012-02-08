using UnityEngine;
using System.Collections;

public class Topo : BaseAI {
	
	/// <summary>
	/// Este script hace uso de la clase BaseAI que utilizo en todos mis proyectos para crear una sencilla maquina de estados. Aun se puede mejorar,
	/// pero en este caso es más que suficiente
	/// </summary>
	
	Vector3 PosicionOriginal; //La posición en la que empezamos
	Vector3 PosicionArriba; // Posición cuando el "topo" este arriba
	Transform thisTransform; //Componente transfrom propio. Esto se define como cache de un componente. Utilizar transform en tiempo de ejecución
							// consume mucho más que si lo asignamos a una variable en Start
	float RestaTiempo = 0;
	
	
	public Texture2D[] Texturas; //El array con las texturas para los distintos tipos de topo
	

	// Use this for initialization
	void Start () {
		// Como hemos visto más arriba, hacemos una cache del componente transform de este GameObject
		thisTransform = transform;
		
		//Inicializamos la posición inicial
		PosicionOriginal = thisTransform.position;
		
		//Calculamos la posición del "topo" fuera
		PosicionArriba = new Vector3(thisTransform.position.x,thisTransform.position.y + 2,thisTransform.position.z);
		
		//Este método es parte de la clase BaseAI, nos permite cambiar de un estado a otro definiendo que método queremos que se ejecute durante ese estado y durante cuanto tiempo
		ChangeState(EsperarAbajo,2f);
	
	}
	
	// Update is called once per frame
	void Update () {
		//Esta parte es necesaria para la maquina de estados, ejecuta el estado de cada momento dentro de Update
		if(stateAction != null)
		{
			stateAction();
		}
		
		//Controles que simplemente aceleran la velocidad del juego. Time.timeSinceLevelLoad nos da el tiempo desde que se ha cargado el nivel en el que estamos
		if(Time.timeSinceLevelLoad > 10 && Time.timeSinceLevelLoad < 19)
		{
			RestaTiempo = 0.2f;
		}
		else if(Time.timeSinceLevelLoad > 20 && Time.timeSinceLevelLoad < 29)
		{
			RestaTiempo = 0.3f;
		}
			
	
	}
	/// <summary>
	/// Utilizando este método asignamos en tiempo de ejecución una nueva textura al material del "topo" 
	/// Para ello utilizamos un entero aletario que determinara que color de textura asignamos
	/// </summary>
	void CambiarColor()
	{
		int RandomValue = Random.Range(0,Texturas.Length);
		renderer.material.mainTexture = Texturas[RandomValue];
	}
	
	
	
	/// <summary>
	/// Este es uno de nuestro estados, encargado de hacer salir al "topo" . Utilizamos iTween para el movimiento, y nuestro método CambiarColor() para asignar un nuevo color
	/// </summary>
	void Salir()
	{
		if(initialising)
		{
			CambiarColor();
			iTween.MoveTo(gameObject,iTween.Hash("position",PosicionArriba,"time",1 - RestaTiempo,"oncomplete","CambiarArriba"));
		}
	}
	
	
	/// <summary>
	/// Este estado se encarga de devolver al "topo" a su posición inicial
	/// </summary>
	void Entrar()
	{
		if(initialising)
		{
			iTween.MoveTo(gameObject,iTween.Hash("position",PosicionOriginal,"time",1 - RestaTiempo,"oncomplete","CambiarAbajo"));
		}
	}
	
	/// <summary>
	/// Ambos métodos, CambiarArriba y CambiarAbajo se encargan de todos los cambios necesarios entre un estado y otro, dando una aleatoriedad al tiempo de estar
	/// abajo
	/// </summary>
	public void CambiarArriba()
	{
		ChangeState(EsperarArriba,0.5f);
	}
	
	public void CambiarAbajo()
	{
		float RandomValue = Random.Range(5,60) * 0.1f;
		ChangeState(EsperarAbajo,RandomValue - RestaTiempo);
	}
	
	
	/// <summary>
	/// Estos dos estados simplemente hacen al "topo" esperar, tanto arriba como abajo. Utilizamos la variable TimeScale, de la clase BaseAI que nos devuelve un valor 
	/// de 0 a 1 indicandonos cuanto tiempo del indicado para este estado ha transcurrido. Es decir, TimeScale > 1 nos indica que el tiempo determinado para
	/// ese estado ha pasado. Los estados no cambian automaticamente
	/// </summary>
	
	void EsperarArriba()
	{
		if(TimeScale > 1)
		{
			ChangeState(Entrar,1f);
		}
	}
	
	void EsperarAbajo()
	{
		if(TimeScale > 1)
		{
			ChangeState(Salir,1f);
		}
	}
	
	/// <summary>
	/// Esta función es llamada desde PointsController, y devuelve un valor entero indicando los puntos conseguidos al pulsar sobre este "topo"
	/// Comprobamos si este "topo" es del color correcto o no, si es correcto devolvemos un punto, si no devolvemos -1 indicando a PointsController
	/// que el jugador ha fallado y debe perder un punto
	/// </summary>
	public int Selected()
	{
		transform.position = PosicionOriginal;
		if(renderer.material.mainTexture == Texturas[1])
		{
			
			CambiarAbajo();
			ChangeState(Entrar,1f - RestaTiempo);
			return 1;
		}
		else
		{
			CambiarAbajo();
			ChangeState(Entrar,1f - RestaTiempo);
			return -1;
		}
	}
	
	
		
}
