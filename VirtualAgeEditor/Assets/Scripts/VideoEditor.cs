using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoEditor : MonoBehaviour {

    public GameObject videoEdit;
    public GameObject timeline;
    public GameObject infoBar;
    public GameObject numbers;
    List<GameObject> timing = new List<GameObject>();
    List<float> timingValue = new List<float>();
    List<GameObject> FirstLayer = new List<GameObject>();
    List<GameObject> SecondLayer = new List<GameObject>();
    List<GameObject> ThirdLayer = new List<GameObject>();
    List<GameObject> FourthLayer = new List<GameObject>();
    List<GameObject> FifthLayer = new List<GameObject>();
    public Slider slider;
    float num;
    float pos = 0;
    float scale = 1;

    private void Start() {
        num = 4;
        StartingTimeLineNumbers();
    }


    private void Update() {
        ScrollZoom();


    }

    List<GameObject> TimeLineNumbers() {

        //para añadir los numeros al hacer zoom in
        Debug.Log("d");

        List<GameObject> Temptiming = new List<GameObject>(); //lista temporal para almacenar los nuevos numeros
        if (Temptiming != null) {
            Temptiming.Clear();
        }

        for (int i = 0; i < timing.Count - 1; i++) {
            float numA = float.Parse(timing[i].name); //valor del primer numero a comparar
            float numB = float.Parse(timing[i + 1].name); //valor del segundo numero

            float newNum = ((numB - numA) / 2); // el numero a añadir sera la mitad de los colindantes ej: 120-60/2 = 30
            GameObject text = new GameObject("text");
            text.name = (numA + newNum).ToString(); //le ponemos como nombre el valor
            text.AddComponent<Text>();
            if (numA % 60 == 0) { // si los numeros son minutos enteros, dividimos entre 60 para tener los segundos
                text.GetComponent<Text>().text = ((numA / 60) + (newNum / 100)).ToString();
            } else { // si ya son segundos, solo dividimos entre 100 para tener el valor en decimal
                text.GetComponent<Text>().text = ((numA / 100) + (newNum / 100)).ToString();
            }

            text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.GetComponent<Text>().fontSize = 50;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            text.transform.SetParent(numbers.transform);

            float Xpos = (timing[i + 1].transform.localPosition.x - timing[i].transform.localPosition.x) / 2;
            text.transform.localPosition = new Vector3(timing[i].transform.localPosition.x + Xpos, 0, 0);
            text.transform.localPosition = new Vector3(text.transform.localPosition.x, 0, 0);
            Temptiming.Add(text);
            GameObject line = new GameObject("line");
            line.AddComponent<Image>();
            line.AddComponent<RectTransform>();
            line.GetComponent<Image>().color = Color.white;
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(6, 500);
            line.transform.SetParent(text.transform);
            line.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            line.transform.localPosition = new Vector3(0, -100, 0);
            line.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            line.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        }


        foreach (GameObject t in Temptiming) {
            timing.Add(t); // añadimos ahora todos los numeros nuevos a la lista global
        }
        timing.Sort((p1, p2) => float.Parse(p1.name).CompareTo(float.Parse(p2.name))); // ordenamos la lista global de numero inferior a superior

        return Temptiming;
    }



    void StartingTimeLineNumbers() {

        //numeros con los que empieza la timeline (5min)
        float dist = timeline.GetComponent<RectTransform>().sizeDelta.x / num; //distancia a la que tienen que estar los numeros

        for (int i = 0; i < num + 1; i++) {
            GameObject text = new GameObject("text"); //creamos los numeros
            text.name = (i * 60).ToString(); // el nombre es el tiempo en segundos
            text.AddComponent<RectTransform>();
            text.AddComponent<Text>();
            text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.GetComponent<Text>().fontSize = 50;
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

            float time = i;
            text.GetComponent<Text>().text = time.ToString() + ".00"; //ponemos el texto
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

            text.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            text.transform.localPosition = new Vector3(pos, 0, 0);

            text.transform.SetParent(numbers.transform);
            text.transform.localPosition = new Vector3(text.transform.localPosition.x, 0, 0);
            timing.Add(text); //añadimos el objeto a la lista global
            GameObject line = new GameObject("line");
            line.AddComponent<Image>();
            line.AddComponent<RectTransform>();
            line.GetComponent<Image>().color = Color.red;
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(8, 500);
            line.transform.SetParent(text.transform);
            line.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            line.transform.localPosition = new Vector3(0, -100, 0);
            line.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            line.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);



        }
    }



    public void ZoomInButton(int num) {

        float value = numbers.transform.localScale.x;

        if (value < 15) { //zoom in maximo que podemos hacer
            numbers.transform.localScale = new Vector3(numbers.transform.localScale.x + num, 1, 1); //Se hace grande el panel donde estan los numeros
            foreach (GameObject obj in timing) {
                obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //Modificamos la escala de los numeros para que no se deforme
            }
            value = numbers.transform.localScale.x;
            scale = numbers.transform.localScale.x;

            //Creamos los numeros y los vamos añadiendo a diferentes listas para clasificarlos por capas de detalle
            if (scale > 1 && scale <= 2f) {
                Debug.Log("1");

                FirstLayer = TimeLineNumbers();
                scale = numbers.transform.localScale.x;
            } else if (scale > 2 && scale <= 3) {
                Debug.Log("2");

                SecondLayer = TimeLineNumbers();
                scale = numbers.transform.localScale.x;
            } else if (scale > 3 && scale <= 4) {
                Debug.Log("3");

                ThirdLayer = TimeLineNumbers();
                scale = numbers.transform.localScale.x;
            } else if (scale > 4 && scale <= 5) {
                Debug.Log("4");

                FourthLayer = TimeLineNumbers();
                scale = numbers.transform.localScale.x;
            } else if (scale > 7 && scale <= 15) {
                Debug.Log("5");
                if (FifthLayer.Count == 0) {
                    FifthLayer = TimeLineNumbers();
                    scale = numbers.transform.localScale.x;
                }

            }

        }



    }


    public void ZoomOutButton(int num) {
        float value = numbers.transform.localScale.x;

        if (value > 2) { //zoom out maximo que podemos hacer
            if (value > 10 && value < 15) {
                numbers.transform.localScale = new Vector3(10, 1, 1); //Se hace grande el panel donde estan los numeros
            } else {
                numbers.transform.localScale = new Vector3(numbers.transform.localScale.x - num, 1, 1); //Se hace grande el panel donde estan los numeros
            }
            foreach (GameObject obj in timing) {
                obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //Modificamos la escala de los numeros para que no se deforme
            }
            value = numbers.transform.localScale.x;
            Debug.Log("Scale: " + scale);
            //Creamos los numeros y los vamos añadiendo a diferentes listas para clasificarlos por capas de detalle
            if (scale > 1 && scale <= 2f) {
                scale = numbers.transform.localScale.x;
                Debug.Log("1");

                foreach (GameObject t in FirstLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FirstLayer.Clear();
            } else if (scale > 2 && scale <= 3) {
                Debug.Log("2");

                scale = numbers.transform.localScale.x;

                foreach (GameObject t in SecondLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                SecondLayer.Clear();
            } else if (scale > 3 && scale <= 4) {
                Debug.Log("3");

                scale = numbers.transform.localScale.x;

                foreach (GameObject t in ThirdLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                ThirdLayer.Clear();
            } else if (scale > 4 && scale < 7) {
                Debug.Log("4");

                scale = numbers.transform.localScale.x;

                foreach (GameObject t in FourthLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FourthLayer.Clear();
            } else if (scale >= 7 && scale <= 15) {
                Debug.Log("5");

                scale = numbers.transform.localScale.x;

                foreach (GameObject t in FifthLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FifthLayer.Clear();

            }

        } else {
            numbers.transform.localScale = new Vector3(1, 1, 1);
            foreach (GameObject obj in timing) {
                obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //Modificamos la escala de los numeros para que no se deforme
            }
            if (FirstLayer.Count > 0) {
                foreach (GameObject t in FirstLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FirstLayer.Clear();
            }

        }
    }


    void ScrollZoom() {
        float value = numbers.transform.localScale.x;



        //Para crear los numeros con más detalle al hacer zoom in


        if (Input.GetAxis("Mouse ScrollWheel") < -0.1f) {
            if (value < 15) { //zoom in maximo que podemos hacer
                numbers.transform.localScale = new Vector3(numbers.transform.localScale.x + 0.1f, 1, 1); //Se hace grande el panel donde estan los numeros
                foreach (GameObject obj in timing) {
                    obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //Modificamos la escala de los numeros para que no se deforme
                }

                //Creamos los numeros y los vamos añadiendo a diferentes listas para clasificarlos por capas de detalle
                if (value > 1 && value < 1.5f) {

                    if (numbers.transform.localScale.x > scale + 0.4f) {

                        FirstLayer = TimeLineNumbers();
                        scale = numbers.transform.localScale.x;
                    }


                } else if (value > 1.5f && value < 3f) {

                    if (numbers.transform.localScale.x > scale + 1f) {
                        SecondLayer = TimeLineNumbers();
                        scale = numbers.transform.localScale.x;
                    }


                } else if (value > 3.1f && value < 4.2f) {

                    if (numbers.transform.localScale.x > scale + 1f) {
                        ThirdLayer = TimeLineNumbers();
                        scale = numbers.transform.localScale.x;
                    }


                } else if (value > 4.3f && value < 5.4f) {

                    if (numbers.transform.localScale.x > scale + 1f) {
                        FourthLayer = TimeLineNumbers();
                        scale = numbers.transform.localScale.x;
                    }

                } else if (value > 7f && value < 8f) {
                    if (FifthLayer.Count == 0) {
                        FifthLayer = TimeLineNumbers();
                    }

                }

            }
        }


        //Para eliminar los numeros cuando hacemos zoom out


        if (Input.GetAxis("Mouse ScrollWheel") > 0.1f) {
            if (value > 1) { //zoom out maximo que podemos hacer
                numbers.transform.localScale = new Vector3(numbers.transform.localScale.x - 0.1f, 1, 1); //Se hace pequeño el panel donde estan los numeros
                foreach (GameObject obj in timing) {
                    obj.transform.localScale = new Vector3(1 / numbers.transform.localScale.x, 1, 1); //modificamos la escala de los numeros para que no se deformen
                }
            }

            //Vamos eliminando los numeros por capas de detalle

            if (value > 1f && value < 1.5f) {
                scale = numbers.transform.localScale.x;
                foreach (GameObject t in FirstLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FirstLayer.Clear();

            } else if (value > 1.5f && value < 2f) {
                scale = numbers.transform.localScale.x;
                foreach (GameObject t in SecondLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                SecondLayer.Clear();

            } else if (value > 3f && value < 4f) {
                scale = numbers.transform.localScale.x;
                foreach (GameObject t in ThirdLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                ThirdLayer.Clear();

            } else if (value > 4f && value < 5f) {
                scale = numbers.transform.localScale.x;
                foreach (GameObject t in FourthLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FourthLayer.Clear();

            } else if (value > 7f && value < 8f) {
                scale = numbers.transform.localScale.x;

                foreach (GameObject t in FifthLayer) {
                    timing.Remove(t);
                    Destroy(t);
                }
                FifthLayer.Clear();

            }
        }
    }
}
