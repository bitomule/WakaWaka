using UnityEngine;
using System.Collections;

public class PointsController : MonoBehaviour {
	/// <summary>
	/// La variable Puntos almacena el número de puntos para mostrarlos por pantalla.En este ejemplo no tienen mas funcionalidad, pero podríamos poner un límite que 
	/// nos hiciese ganar el juego
	/// </summary>
	
	int Puntos = 0;

	
	// Update is called once per frame
	void Update () {
		//Dentro de Update vamos a lanzar un Raycast desde la posición de la pantalla para saber si hemos dado a unos de nuestros "supuestos" topos
		
		//Comprobamos si estamos pulsando el botón izquierdo del ratón
		if(Input.GetMouseButtonDown(0))
		{
			//Creamos el Ray desde la cámara. Los parámetros deben ser siempre la posición en X del ratón o del dedo en caso de pantalla táctil,
			// El segundo parámetro es la posición en y el último en este caso es indiferente, pero seria la distancia desde la pantalla hacia delante
			// a la que se dispara el rayo. Su origen
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y,100));
			
			//Variable info que pasamos por referencia. Una variable que se pasa por referencia permite a un método cambiar el valor de esa variable
			RaycastHit info;
			
			//Lanzamos el RayCast de la forma má sencilla
			if(Physics.Raycast(ray,out info))
			{
				//Obtenemos el GameObject con el que colisione el raycast, y de el obtenemos el Componente Topo, el script que tendra cada uno de ellos
				//Este es un ejemplo de como obtener desde un script una referencia a otro script, para acceder a sus variables y métodos públicos.
				
				Topo TopoSeleccionado = (Topo)info.transform.gameObject.GetComponent<Topo>();
				
				//En el caso de que el objeto seleccionado sea un topo, es decir, que no sea null su referencia. Esto se podría hacer también utilizando tags
				if(TopoSeleccionado != null)
				{
					//Añadimos a la variable de puntos el valor retornado por el método Selected de nuestro script TopoSeleccionado
					Puntos+= TopoSeleccionado.Selected();
				}
			}
			                                       
			
		}
	
	}
	
	
	void OnGUI()
	{
		//Label sencilla para pintar los puntos
		GUI.Label( new Rect(10,10,100,30),"PUNTOS: " + Puntos);
	}
}
