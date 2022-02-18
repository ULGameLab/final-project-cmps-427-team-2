namespace FireChickenGames.Combat.Editor
{
    using GameCreator.ModuleManager;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Attacker))]
    public class AttackerEditor : Editor
    {
        protected Attacker instance;

        public SerializedProperty weaponStashGameObject;

        // Aiming.
        public SerializedProperty aimKey;
        public SerializedProperty canAim;
        public SerializedProperty canRunWhileAiming;
        public SerializedProperty zoomInWhileAiming;
        public SerializedProperty zoomInAimingFieldOfView;
        public SerializedProperty zoomInAimingTransitionDurationInSeconds;
        public SerializedProperty zoomOutAimingFieldOfView;
        public SerializedProperty zoomOutAimingTransitionDurationInSeconds;

        // Shooter Weapon Management.
        public SerializedProperty shooterAttackKey;
        public SerializedProperty drawWeaponKey;
        public SerializedProperty cycleAmmoKey;
        public SerializedProperty cycleFireModeKey;
        public SerializedProperty cycleWeaponKey;
        public SerializedProperty reloadAmmoKey;
        public SerializedProperty dropWeaponKey;

        // Melee Weapon Management.
        public SerializedProperty meleeAttackKey;
        public SerializedProperty unsheatheWeaponKey;
        public SerializedProperty blockKey;
        public SerializedProperty evadeKey;
        public SerializedProperty evasionInvincibilityDuration;
        public SerializedProperty actionCharacterDash;

        protected void OnEnable()
        {
            instance = target as Attacker;

            weaponStashGameObject = serializedObject.FindProperty("weaponStashGameObject");

            canRunWhileAiming = serializedObject.FindProperty("canRunWhileAiming");
            zoomInWhileAiming = serializedObject.FindProperty("zoomInWhileAiming");
            zoomInAimingFieldOfView = serializedObject.FindProperty("zoomInAimingFieldOfView");
            zoomInAimingTransitionDurationInSeconds = serializedObject.FindProperty("zoomInAimingTransitionDurationInSeconds");
            zoomOutAimingFieldOfView = serializedObject.FindProperty("zoomOutAimingFieldOfView");
            zoomOutAimingTransitionDurationInSeconds = serializedObject.FindProperty("zoomOutAimingTransitionDurationInSeconds");

            aimKey = serializedObject.FindProperty("aimKey");
            shooterAttackKey = serializedObject.FindProperty("shooterAttackKey");
            drawWeaponKey = serializedObject.FindProperty("drawWeaponKey");
            cycleAmmoKey = serializedObject.FindProperty("cycleAmmoKey");
            cycleFireModeKey = serializedObject.FindProperty("cycleFireModeKey");
            cycleWeaponKey = serializedObject.FindProperty("cycleWeaponKey");
            reloadAmmoKey = serializedObject.FindProperty("reloadAmmoKey");
            dropWeaponKey = serializedObject.FindProperty("dropWeaponKey");

            meleeAttackKey = serializedObject.FindProperty("meleeAttackKey");
            unsheatheWeaponKey = serializedObject.FindProperty("unsheatheWeaponKey");
            blockKey = serializedObject.FindProperty("blockKey");
            evadeKey = serializedObject.FindProperty("evadeKey");
            evasionInvincibilityDuration = serializedObject.FindProperty("evasionInvincibilityDuration");
            actionCharacterDash = serializedObject.FindProperty("actionCharacterDash");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(weaponStashGameObject, new GUIContent("Weapon Stash"));
            EditorGUILayout.Space();

            // Shooter.
            EditorGUILayout.LabelField("Shooter", EditorStyles.boldLabel);
            var isShooterCombatEnabled = ModuleManager.IsEnabled(ModuleManager.GetModuleManifest("com.firechickengames.module.shootercombat").module);
            if (!isShooterCombatEnabled)
                EditorGUILayout.HelpBox(
                    "The Shooter module and Combat (Shooter) integration modules are both required shooter attacker functionality.",
                    MessageType.Info,
                    false
                );
            EditorGUI.BeginDisabledGroup(!isShooterCombatEnabled);
            // Shooter Weapon Management.
            EditorGUILayout.LabelField("User Input");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(aimKey, new GUIContent("Aim"));
            EditorGUILayout.PropertyField(shooterAttackKey, new GUIContent("Attack"));
            EditorGUILayout.PropertyField(drawWeaponKey, new GUIContent("Draw/Holster"));
            EditorGUILayout.PropertyField(reloadAmmoKey, new GUIContent("Reload"));
            EditorGUILayout.PropertyField(cycleAmmoKey, new GUIContent("Cycle Ammo"));
            EditorGUILayout.PropertyField(cycleFireModeKey, new GUIContent("Cycle Fire Mode"));
            EditorGUILayout.PropertyField(cycleWeaponKey, new GUIContent("Cycle Weapon"));
            EditorGUILayout.PropertyField(dropWeaponKey, new GUIContent("Drop Weapon"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            // Aiming.
            EditorGUILayout.LabelField("Aiming");

            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(canRunWhileAiming);

            EditorGUILayout.PropertyField(zoomInWhileAiming, new GUIContent("Zoom While Aiming"));
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(!zoomInWhileAiming.boolValue);

            EditorGUILayout.LabelField("Zoom In");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(zoomInAimingFieldOfView, new GUIContent("Field Of View"));
            EditorGUILayout.PropertyField(zoomInAimingTransitionDurationInSeconds, new GUIContent("Transition Duration (seconds)"));
            EditorGUI.indentLevel--;

            
            EditorGUILayout.LabelField("Zoom Out");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(zoomOutAimingFieldOfView, new GUIContent("Field Of View"));
            EditorGUILayout.PropertyField(zoomOutAimingTransitionDurationInSeconds, new GUIContent("Transition Duration (seconds)"));
            EditorGUI.indentLevel--;

            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            // Melee.
            EditorGUILayout.LabelField("Melee", EditorStyles.boldLabel);
            var isMeleeCombatEnabled = ModuleManager.IsEnabled(ModuleManager.GetModuleManifest("com.firechickengames.module.meleecombat").module);
            EditorGUI.BeginDisabledGroup(!isMeleeCombatEnabled);
            EditorGUILayout.LabelField("User Input");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(meleeAttackKey, new GUIContent("Attack"));
            EditorGUILayout.PropertyField(blockKey, new GUIContent("Block"));
            EditorGUILayout.PropertyField(unsheatheWeaponKey, new GUIContent("Unsheathe/Sheathe"));
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();
            if (!isMeleeCombatEnabled)
                EditorGUILayout.HelpBox(
                    "The Melee module and Combat (Melee) integration modules are both required melee attacker functionality.",
                    MessageType.Info,
                    false
                );

            // Evasion
            EditorGUILayout.LabelField("Evasion", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("User Input");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(evadeKey, new GUIContent("Evade"));
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Actions");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(actionCharacterDash, new GUIContent("On Evade"));
            EditorGUI.indentLevel--;


            EditorGUI.BeginDisabledGroup(!isMeleeCombatEnabled);
            EditorGUILayout.PropertyField(evasionInvincibilityDuration, new GUIContent("Evasion Invincibility Duration (seconds)"));
            EditorGUI.EndDisabledGroup();
            if (!isMeleeCombatEnabled)
            {
                EditorGUILayout.HelpBox(
                    "The Melee module and Combat (Melee) integration modules are both required melee invincibility during evasion.",
                    MessageType.Info,
                    false
                );
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
