using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
namespace PipeFitter.Assembly
{
    public enum PEffectState
    {
        None = 0,
        MovingToLocation=1,
        Active=2,
        Finishing=3,
    }
    public class PFEffect : MonoBehaviour
    {
        [Tooltip("Our root item that has everything on it")]
        public GameObject RootEffectPivot;
        public PEffectState EffectState = PEffectState.None;
        [Space]
        [Tooltip("If there's an object in our environment we want to turn on off")]
        public GameObject WorldObjectRef;
        [Tooltip("If we wanted to activate this for the time of the tool")]
        public GameObject FirstPersonObjectRef;
        public float TimeToMoveToLocation = 1f;
        [SerializeField] protected float movingTime = 0;
        public float TimeForEvent = 1.5f;
        protected WaitForSeconds EffectDelay;
        protected WaitForSeconds TimeToMoveDelay;
       
        protected Vector3 startLocation;
        protected Vector3 endLocation;
        public List<ParticleSystem> Effects = new List<ParticleSystem>();
        public List<AudioSource> AudioSources = new List<AudioSource>();
        public UnityEvent OnEffectsFinished;
        protected Coroutine effectDelayRoutine;
        protected bool effectActive;
        public virtual void Awake()
        {
            EffectDelay = new WaitForSeconds(TimeForEvent);
            TimeToMoveDelay = new WaitForSeconds(TimeToMoveToLocation);
            startLocation = RootEffectPivot.transform.position;
        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
        public void SetupEffectDetails(Vector3 worldPosition)
        {
            endLocation = worldPosition; 
            //RootEffectPivot.transform.position = worldPosition;
        }
        public void RunEffect(Vector3 worldPosition)
        {
            movingTime = 0;
            endLocation = worldPosition;
            effectActive = true;
            RootEffectPivot.SetActive(true);
            EffectState = PEffectState.MovingToLocation;
            if (WorldObjectRef != null)
            {
                WorldObjectRef.SetActive(false);
            }
            if (effectDelayRoutine != null)
            {
                StopCoroutine(effectDelayRoutine);
                StopEffect();
                effectDelayRoutine = null;
            }
           
            effectDelayRoutine = StartCoroutine(TimeDelayEndEffect());
        }
        public void StopEffect() 
        {
            foreach (ParticleSystem p in Effects)
            {
                p.Stop();
            }
            for (int i = 0; i < AudioSources.Count; i++)
            {
                var source = AudioSources[i];
                source.Stop();
            }
        }
        IEnumerator TimeDelayEndEffect()
        {
           
            yield return new WaitForSeconds(TimeToMoveToLocation);
            foreach (ParticleSystem p in Effects)
            {
                p.Play();
            }
            for(int i = 0; i < AudioSources.Count; i++)
            {
                var source = AudioSources[i];
                source.Play();
            }
            yield return new WaitForSeconds(TimeForEvent);
            StopEffect();
            EffectState = PEffectState.Finishing;
            yield return new WaitForSeconds(TimeToMoveToLocation);
            effectDelayRoutine = null;
            OnEffectsFinished.Invoke();
            effectActive = false;
            RootEffectPivot.SetActive(false);
            if (WorldObjectRef != null)
            {
                WorldObjectRef.SetActive(true);
            }
            //RootEffectPivot.transform.position = startLocation;
        }
        /// <summary>
        /// Do some sort of tool activation work
        /// </summary>
        public void ToolActivated()
        {
            if (FirstPersonObjectRef != null)
            {
                FirstPersonObjectRef.SetActive(true);
            }
        }
        public void ToolDeactivated()
        {
            if (FirstPersonObjectRef != null)
            {
                FirstPersonObjectRef.SetActive(false);
            }
        }
        public void Update()
        {
            if (effectActive)
            {
                switch (EffectState)
                {
                    case PEffectState.MovingToLocation:
                        var ratio = (float) movingTime / TimeToMoveToLocation;
                        if (ratio >= 1)
                        {
                            EffectState = PEffectState.Active;
                            movingTime = 0;
                            break;
                        }
                        RootEffectPivot.transform.position = Vector3.Lerp(startLocation, endLocation, ratio);
                        movingTime += Time.deltaTime;
                        break;
                    case PEffectState.Active:
                        RootEffectPivot.transform.position = endLocation;
                        movingTime = 0;
                        break;
                    case PEffectState.Finishing:
                        var finRatio = (float)movingTime / TimeToMoveToLocation;
                        RootEffectPivot.transform.position = Vector3.Lerp(endLocation, startLocation, finRatio);
                        movingTime += Time.deltaTime;
                        break;
                }
            }
        }
    }
}
