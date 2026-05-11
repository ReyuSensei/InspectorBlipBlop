using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI progressText;



    void Start()
    {
        //ARRANCO LA CORUTINA DE CARGA ASINCRONA
        //UNA CORUTINA SE VA EJECUTANDO DE FORMA SEMIPARALELA (SE EJECUTA CUANDO ENCUENTRA HUECO)
        StartCoroutine(LoadAsync());

        //currentBackground = 
    }


    //IENUMERATOR ES UN TIPO DE VARIABLE DE ITERACION QUE CONTROLA EL BUCLE DE EJECUCION DE LA CORUTINA
    private IEnumerator LoadAsync()
    {
        //EMPIEZO LA CARGA ASINCRONA DE LA ESCENA QUE INDIQUE EL SINGLETON
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameManager.instance.nextLevelToLoad);
        //NO MUESTRO LA ESCENA HASTA QUE ESTE CARGADA
        asyncLoad.allowSceneActivation = false;
        //UNA CORUTINA SE EJECUTA DURANTE UN BUCLE O UNA ESPERA
        //LA CORUTINA SE EJECUTARA MIENTRAS NO SE HAYA FINALIZADO TODA LA CARGA
        while(asyncLoad.isDone == false)
        {
            progressText.text = "Loading... " + (asyncLoad.progress * 100) + "%";
            //LA CARGA ASINCRONA NUNCA LLEGA AL 100%
            //PARA DECIDIR QUE ACTIVAMOS LA ESCENA CARGADA LO TENEMOS QUE HACER AL LLEGAR AL 90%
            if(asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            //LE TENGO QUE INDICAR AL LOOP O A LA CORUTINA CUANTO QUIERO QUE ESPERE
            yield return null;  //ESPERATE "NADA" PARA SEGUIR
        }

    }

}
