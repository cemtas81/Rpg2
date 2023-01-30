using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private List<Material> materials = new List<Material>();
        [SerializeField]
        private float value;

        void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }
            SetValue(0);
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        // Update is called once per frame
        void Update()
        {
     

            if (Input.GetKeyDown( KeyCode.E))
            {
               
                if (value==0)
                {
                  
                    StartCoroutine(LerpFunction(1, 1));

                }
            
            }
          
        }
        IEnumerator LerpFunction(float endValue, float duration)
        {
            float time = 0;
            float startValue = value;
            while (time < duration)
            {
                value = Mathf.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                SetValue(value);
                yield return null;
            }
            value = endValue;
            yield return new WaitForSecondsRealtime(5);
           
            if (value==1)
            {
                StartCoroutine(LerpFunction(0, 1));
            }
           
        }


        public void SetValue(float value)
        {
           
                for (int i = 0; i < materials.Count; i++)
                {
                    materials[i].SetFloat("_Dissolve", value);
                }
         
          
          
        }
    }
}