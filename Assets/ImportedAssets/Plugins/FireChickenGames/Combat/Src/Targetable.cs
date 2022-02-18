namespace FireChickenGames.Combat
{
    using FireChickenGames.Combat.Core;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Animations;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [AddComponentMenu("Fire Chicken Games/Combat/Targetable")]
    public class Targetable : MonoBehaviour
    {
        [Tooltip("Indicates if a targetable can in fact be targeted. If not set, this setting will be ignored.")]

        [Header("Targetability")]
        [VariableFilter(Variable.DataType.Bool)]
        public VariableProperty canBeTargeted = new VariableProperty(Variable.VarType.LocalVariable);

        [Tooltip("Used to determine if the mesh is visible. If not set, the first renderer found in child objects is used.")]
        public Renderer visibilityDetectionRenderer;

        [Header("Active Target Indicator")]
        [Tooltip("If enabled, a target indicator will appear near the targetable object while it is targeted.")]
        public bool showIndicator = true;
        [Tooltip("The prefab to display above a targetable when it is the active target. If left blank, a default game creator Floating Message prefab will be used. A custom prefab needs to only have a Text component assigned to one of its children.")]
        public GameObject indicatorPrefab;
        [Tooltip("The position, relative to the targetable game object, where the indicator prefab is displayed.")]
        public Vector3 indicatorOffset = new Vector3(0, 2.1f, 0);
        [VariableFilter(Variable.DataType.String)]
        [Tooltip("The text to populate the indicator with.")]
        public VariableProperty indicatorTextContent = new VariableProperty(Variable.VarType.LocalVariable);
        [Tooltip("The color of the indicator prefab's text.")]
        public Color indicatorTextColor = Color.white;

        [Header("Mouse Selection")]
        [Tooltip("If enabled, the targetable can be selected with the mouse (provided that the targeter has mouse input enabled as well).")]
        public bool isMouseTargetSelectionEnabled = true;

        [Header("Mouse Hover Indicator")]
        [Tooltip("If enabled, a target indicator will appear near the targetable object while it is targeted.")]
        public bool showMouseHoverIndicator;
        [Tooltip("The prefab to display above a targetable when it is the active target. If left blank, a default game creator Floating Message prefab will be used. A custom prefab needs to only have a Text component assigned to one of its children.")]
        public GameObject mouseHoverIndicatorPrefab;
        [Tooltip("The position, relative to the targetable game object, where the indicator prefab is displayed.")]
        public Vector3 mouseHoverIndicatorOffset = new Vector3(0, 2.1f, 0);
        [VariableFilter(Variable.DataType.String)]
        [Tooltip("The text to populate the indicator with.")]
        public VariableProperty mouseHoverIndicatorTextContent = new VariableProperty(Variable.VarType.LocalVariable);
        [Tooltip("The color of the indicator prefab's text.")]
        public Color mouseHoverIndicatorTextColor = Color.white;

        [Header("Actions")]
        [Tooltip("Actions that are executed when the targetable becomes the active target. If not set, this setting will be ignored.")]
        public Actions onBecomeActiveTargetActions;
        [Tooltip("Actions that are executed when some other targetable becomes the active target. If not set, this setting will be ignored.")]
        public Actions onNotActiveTargetActions;
        [Tooltip("Actions that are periodically executed while the targetable continues to be targeted. If not set, this setting will be ignored.")]
        public Actions onContinuingToBeTargetedActions;
        
        private Targeter _targeter;
        public Targeter Targeter
        {
            get
            {
                if (_targeter == null)
                    _targeter = HookManager.GetPlayerTargeter();
                return _targeter;
            }
        }

        private const string CANVAS_ASSET_PATH = "GameCreator/Messages/FloatingMessage";
        private GameObject activeTargetIndicatorInstance;
        private GameObject mouseHoverIndicatorInstance;

        // Events
        public class OnTargetChangedEvent : UnityEvent<GameObject> { }
        public static OnTargetChangedEvent onTargetChangedEvent = new OnTargetChangedEvent();
        public class OnContinuingToBeTargetedEvent : UnityEvent<GameObject> { }
        public static OnContinuingToBeTargetedEvent onContinuingToBeTargetedEvent = new OnContinuingToBeTargetedEvent();

        void Start()
        {
            onTargetChangedEvent.AddListener(OnReceiveEvent);
            onContinuingToBeTargetedEvent.AddListener(OnReceiveContinuingToBeTargetedEvent);

            if (visibilityDetectionRenderer == null)
                visibilityDetectionRenderer = GetComponentInChildren<Renderer>();

            if (indicatorPrefab == null)
                indicatorPrefab = InitializeDefaultIndicatorPrefab();


            if (mouseHoverIndicatorPrefab == null)
                mouseHoverIndicatorPrefab = InitializeDefaultIndicatorPrefab();

            mouseHoverIndicatorInstance = Instantiate(mouseHoverIndicatorPrefab, transform);
            mouseHoverIndicatorInstance.SetActive(false);
        }

        void OnDestroy()
        {
            onTargetChangedEvent.RemoveListener(OnReceiveEvent);
            onContinuingToBeTargetedEvent.RemoveListener(OnReceiveContinuingToBeTargetedEvent);
        }

        void Update()
        {
            if ((activeTargetIndicatorInstance ?? false) && !CanBeTargeted())
                Destroy(activeTargetIndicatorInstance);
        }

        void OnMouseOver()
        {
            if (!showMouseHoverIndicator || mouseHoverIndicatorInstance == null)
                return;

            if (Targeter == null || !Targeter.isTargetingEnabled || Targeter.IsCurrentTarget(gameObject))
                return;

            mouseHoverIndicatorInstance.SetActive(true);
            mouseHoverIndicatorInstance.transform.localPosition = mouseHoverIndicatorOffset;

            var text = mouseHoverIndicatorInstance.GetComponentInChildren<Text>();
            text.color = mouseHoverIndicatorTextColor;
            var indicatorTextContentVar = mouseHoverIndicatorTextContent.Get(gameObject);
            text.text = indicatorTextContentVar == null ? "" : mouseHoverIndicatorTextContent.ToStringValue(gameObject);

            if (mouseHoverIndicatorInstance.TryGetComponent<LookAtConstraint>(out var lookAtConstraint))
            {
                lookAtConstraint.SetSources(new List<ConstraintSource>()
                {
                    new ConstraintSource()
                    {
                        sourceTransform = HookCamera.Instance.transform,
                        weight = 1.0f
                    }
                });

                lookAtConstraint.constraintActive = true;
            }
        }

        private void OnMouseExit()
        {
            if (mouseHoverIndicatorInstance != null)
                mouseHoverIndicatorInstance.SetActive(false);
        }

        GameObject InitializeDefaultIndicatorPrefab()
        {
            var databaseGeneral = DatabaseGeneral.Load();
            var prefab = databaseGeneral.prefabFloatingMessage;
            return prefab == null ? Resources.Load<GameObject>(CANVAS_ASSET_PATH) : prefab;
        }

        void ShowIndicator()
        {
            if (!showIndicator)
                return;

            if (activeTargetIndicatorInstance ?? false)
                Destroy(activeTargetIndicatorInstance);

            activeTargetIndicatorInstance = Instantiate(indicatorPrefab, transform);
            activeTargetIndicatorInstance.transform.localPosition = indicatorOffset;

            var text = activeTargetIndicatorInstance.GetComponentInChildren<Text>();
            text.color = indicatorTextColor;
            var indicatorTextContentVar = indicatorTextContent.Get(gameObject);
            text.text = indicatorTextContentVar == null ? "" : indicatorTextContent.ToStringValue(gameObject);

            if (activeTargetIndicatorInstance.TryGetComponent<LookAtConstraint>(out var lookAtConstraint))
            {
                lookAtConstraint.SetSources(new List<ConstraintSource>()
                {
                    new ConstraintSource()
                    {
                        sourceTransform = HookCamera.Instance.transform,
                        weight = 1.0f
                    }
                });

                lookAtConstraint.constraintActive = true;
            }
        }

        protected void OnReceiveEvent(GameObject invoker)
        {
            if (gameObject.GetInstanceID() == invoker?.GetInstanceID())
            {
                ShowIndicator();
                onBecomeActiveTargetActions?.ExecuteWithTarget(invoker);
            }
            else
            {
                if (activeTargetIndicatorInstance ?? false)
                    Destroy(activeTargetIndicatorInstance);
                onNotActiveTargetActions?.ExecuteWithTarget(invoker);
            }
        }

        protected void OnReceiveContinuingToBeTargetedEvent(GameObject invoker)
        {
            if (invoker == null || gameObject.GetInstanceID() != invoker.GetInstanceID())
                return;

            if (onContinuingToBeTargetedActions != null)
                onContinuingToBeTargetedActions.ExecuteWithTarget(invoker);
        }

        /**
         * Public API
         */
        public virtual bool IsVisible()
        {
            if (visibilityDetectionRenderer == null)
                return false;

            var camera = HookManager.GetCamera();
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            var isInViewFrustum = GeometryUtility.TestPlanesAABB(planes, visibilityDetectionRenderer.bounds);
            return isInViewFrustum;
        }

        public virtual bool CanBeTargeted()
        {
            var canBeTargetedVar = canBeTargeted.Get(gameObject);
            var isTargetableByCanBeTarged = canBeTargetedVar == null || (bool)canBeTargetedVar;

            return gameObject.activeInHierarchy && isTargetableByCanBeTarged;
        }

        public static void DispatchTargetChanged(GameObject gameObject = null)
        {
            onTargetChangedEvent.Invoke(gameObject);
        }

        public static void AddListenerOnTargetChanged(UnityAction<GameObject> callback)
        {
            onTargetChangedEvent.AddListener(callback);
        }

        public static void RemoveListenerOnTargetChanged(UnityAction<GameObject> callback)
        {
            onTargetChangedEvent.RemoveListener(callback);
        }

        public static void DispatchContinuingToBeTargeted(GameObject gameObject = null)
        {
            onContinuingToBeTargetedEvent.Invoke(gameObject);
        }

        public static void AddListenerOnContinuingToBeTargeted(UnityAction<GameObject> callback)
        {
            onContinuingToBeTargetedEvent.AddListener(callback);
        }

        public static void RemoveListenerOnContinuingToBeTargeted(UnityAction<GameObject> callback)
        {
            onContinuingToBeTargetedEvent.RemoveListener(callback);
        }
    }
}
