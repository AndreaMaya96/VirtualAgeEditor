using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoEditor : MonoBehaviour {

    public GameObject videoEdit;
    public GameObject timeline;
    public GameObject infoBar;
    public GameObject numbers;
    List<GameObject> timing = new List<GameObject>();
    List<List<GameObject>> Layers = new List<List<GameObject>>();
    public float num;
    float pos = 0;
    float maxDist;
    float minDist;



    private void Start() {

        maxDist = numbers.GetComponent<RectTransform>().sizeDelta.x / 4; //La distancia maxima que puede haber entre un numero y el siguiente
        minDist = numbers.GetComponent<RectTransform>().sizeDelta.x / 10; //La distancia minima que puede haber entre un numero y el siguiente

        StartingTimeLineNumbers();
    }


    List<GameObject> TimeLineNumbers() {

        //para añadir los numeros al hacer zoom in

        List<GameObject> Temptiming = new List<GameObject>(); //lista temporal para almacenar los nuevos numeros
        if (Temptiming != null) {
            Temptiming.Clear();
        }

        for (int i = 0; i < timing.Count - 1; i++) {
            float numA = float.Parse(timing[i].name); //valor del primer numero a comparar
            float numB = float.Parse(timing[i + 1].name); //valor del segundo numero

            float newNum = ((numB - numA) / 2) + numA; // calculamos el valor medio entre los dos numeros
            GameObject text = new GameObject("text");
            text.transform.SetParent(numbers.transform);

            text.AddComponent<RectTransform>();
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(70, numbers.GetComponent<RectTransform>().sizeDelta.y);
            text.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1);
            text.name = (newNum).ToString(); //le ponemos como nombre el valor
            text.AddComponent<Text>();
            TimeSpan t = TimeSpan.FromSeconds(newNum);
            text.GetComponent<Text>().text = t.ToString((@"m\:ss")); //Pasamos los segundos a formato m:ss

             
            text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.GetComponent<Text>().fontSize = 20;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            
            text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);

            float Xpos = (timing[i + 1].transform.localPosition.x - timing[i].transform.localPosition.x) / 2; 
            text.transform.localPosition = new Vector3(timing[i].transform.localPosition.x + Xpos, 0, 0); //Colocamos el nuevo numero en medio de los dos 
            text.transform.localPosition = new Vector3(text.transform.localPosition.x, 0, 0);
            Temptiming.Add(text); //Añadimos el numero a la lista temporal
            GameObject line = new GameObject("line"); //Creamos la linea
            line.AddComponent<Image>();
            Color tempColor;
            tempColor = Color.white;
            tempColor.a = 0.5f;
            line.GetComponent<Image>().color = tempColor;
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(2, videoEdit.GetComponent<RectTransform>().sizeDelta.y);
            line.transform.localScale = new Vector3(1, 1, 1);
            line.transform.SetParent(text.transform);
            line.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            line.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -numbers.GetComponent<RectTransform>().sizeDelta.y);
            line.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            line.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        }


        foreach (GameObject t in Temptiming) {
            timing.Add(t); // añadimos ahora todos los numeros nuevos a la lista global
        }
        timing.Sort((p1, p2) => float.Parse(p1.name).CompareTo(float.Parse(p2.name))); // ordenamos la lista global de numero inferior a superior

        if (Vector2.Distance(timing[1].transform.position, timing[0].transform.position) > maxDist) //Si al crear los nuevos numeros, sigue habiendo mucho espacio entre ellos, volvemos a crear mas
        {
            List<GameObject> myList = new List<GameObject>(); //creamos una lista donde almacenar los nuevos numeros
            myList = TimeLineNumbers();
            Layers.Add(myList); //Añadimos la lista de estos nuevos numeros a la lista de listas
        }
        return Temptiming;
    }



    void StartingTimeLineNumbers() {


        float dist = numbers.GetComponent<RectTransform>().sizeDelta.x / num; //distancia a la que tienen que estar los numeros
        for (int i = 0; i < num + 1; i++) {
            float seconds = i * 60;
            GameObject text = new GameObject("text"); //creamos los numeros
            text.name = seconds.ToString(); // el nombre es el tiempo en segundos
            text.AddComponent<RectTransform>();
            text.AddComponent<Text>();

            if (i == 0) { //colocamos los numeros en la posicion que corresponde y movemos el pivot para que el primer y ultimo numero no se salgan de la pantalla
                text.GetComponent<Text>();
                text.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                pos = 0;

            } else {
                pos = pos + dist;
            }
            if (i == num) {
                text.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
            }

            float time = i * 60;
            text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            text.transform.SetParent(numbers.transform);
            text.transform.localPosition = new Vector3(pos, 0, 0);
            text.GetComponent<RectTransform>().sizeDelta = new Vector2(70, numbers.GetComponent<RectTransform>().sizeDelta.y);
            text.transform.localScale = new Vector3(1, 1, 1);

            TimeSpan t = TimeSpan.FromSeconds(time); 
            text.GetComponent<Text>().text = t.ToString((@"m\:ss")); text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter; //Pasamos el tiempo de segundos a formato m:ss
            text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.GetComponent<Text>().fontSize = 20;
            
            timing.Add(text); //añadimos el objeto a la lista global
            GameObject line = new GameObject("line"); //Creamos la linea vertical 
            line.AddComponent<Image>();
            line.GetComponent<Image>().color = Color.red;
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(2, videoEdit.GetComponent<RectTransform>().sizeDelta.y);
            line.transform.SetParent(text.transform);
            line.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            line.transform.localScale = new Vector3(1, 1, 1);
            line.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -numbers.GetComponent<RectTransform>().sizeDelta.y);
            line.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            line.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);

        }


        if (Vector2.Distance(timing[1].transform.position, timing[0].transform.position) > maxDist) //Si al crear los minutos, estan muy separados, creamos más
        {
            List<GameObject> myList = new List<GameObject>();
            myList = TimeLineNumbers();
            Layers.Add(myList);
        }
    }



    public void ZoomInButton(int num) {

            if (timing[1].gameObject.GetComponent<Text>().text != "0:01")
            { //zoom in maximo que podemos hacer
                numbers.transform.localScale = new Vector3(numbers.transform.localScale.x + num, 1, 1); //Se hace grande el panel donde estan los numeros
                foreach (GameObject obj in timing)
                {
                    obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //Modificamos la escala de los numeros para que no se deforme

                }
                if (Vector2.Distance(timing[1].transform.position, timing[0].transform.position) > maxDist)
                {
                    List<GameObject> myList = new List<GameObject>();
                    myList = TimeLineNumbers();
                    Layers.Add(myList);
                }
            }
    }


    public void ZoomOutButton(int num) {
        float value = numbers.transform.localScale.x;
        if (value > 2)
        { //zoom out maximo que podemos hacer
            numbers.transform.localScale = new Vector3(numbers.transform.localScale.x - num, 1, 1); //Se hace pequeño el panel donde estan los numeros
            foreach (GameObject obj in timing)
            {
                obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //modificamos la escala de los numeros para que no se deformen
            }


            if (Vector2.Distance(timing[1].transform.position, timing[0].transform.position) < minDist)
            {

                foreach (GameObject t in Layers[Layers.Count - 1])
                {
                    timing.Remove(t);
                    Destroy(t);
                }
                Layers.RemoveAt(Layers.Count - 1);
            }
        }
        else
        {
            numbers.transform.localScale = new Vector3(1, 1, 1); //Se hace pequeño el panel donde estan los numeros
            foreach(GameObject obj in timing)
            {
                obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //modificamos la escala de los numeros para que no se deformen

            }
            if(Vector2.Distance(timing[1].transform.position, timing[0].transform.position) < minDist)
            {

                foreach (GameObject t in Layers[Layers.Count - 1])
                {
                    timing.Remove(t);
                    Destroy(t);
                }
                Layers.RemoveAt(Layers.Count - 1);
            }
        }
    }
  

    public void ScrollZoom() {
        float value = numbers.transform.localScale.x;

        //Para crear los numeros con más detalle al hacer zoom in
        if (Input.GetAxis("Mouse ScrollWheel") > 0.1f) {
            if (timing[1].gameObject.GetComponent<Text>().text != "0:01") { //zoom in maximo que podemos hacer
                numbers.transform.localScale = new Vector3(numbers.transform.localScale.x + 0.1f, 1, 1); //Se hace grande el panel donde estan los numeros
                foreach (GameObject obj in timing) {
                    obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //Modificamos la escala de los numeros para que no se deforme

                }
                if (Vector2.Distance(timing[1].transform.position, timing[0].transform.position) > maxDist)
                {
                    List<GameObject> myList = new List<GameObject>();
                    myList = TimeLineNumbers();
                    Layers.Add(myList);
                }
            }
        }

        //Para eliminar los numeros cuando hacemos zoom out
         
        if (Input.GetAxis("Mouse ScrollWheel") < -0.1f) {
            if (value > 1)
            { //zoom out maximo que podemos hacer
                numbers.transform.localScale = new Vector3(numbers.transform.localScale.x - 0.1f, 1, 1); //Se hace pequeño el panel donde estan los numeros
                foreach (GameObject obj in timing)
                {
                    obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //modificamos la escala de los numeros para que no se deformen

                }


                if (Vector2.Distance(timing[1].transform.position, timing[0].transform.position) < minDist) //Si los numeros estan demasiado juntos, eliminamos algunos
                {

                    foreach (GameObject t in Layers[Layers.Count - 1]) //Eliminamos los numeros que haya en la ultima lista de la lista Layers, seran los ultimos que hemos creado
                    {
                        timing.Remove(t);
                        Destroy(t);
                    }
                    Layers.RemoveAt(Layers.Count - 1);
                }
            }
        }
    }
}
