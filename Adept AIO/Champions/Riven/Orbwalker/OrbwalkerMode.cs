﻿namespace Adept_AIO.Champions.Riven.Orbwalker
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Menu.Components;
    using Aimtec.SDK.Menu.Config;
    using Aimtec.SDK.Util;

    public class OrbwalkerMode
    {
        #region Fields

        /// <summary>
        ///     The target selection logic for this mode
        /// </summary>
        public TargetDelegate GetTargetImplementation;

        /// <summary>
        ///     This Orbwalker Mode's logic
        /// </summary>
        public OrbwalkModeDelegate ModeBehaviour;

        /// <summary>
        ///     The Orbwalker Instance this mode belongs to
        /// </summary>
        public AOrbwalker ParentInstance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Creates a new instance of an OrbwalkerMode using a Global Key
        /// </summary>
        public OrbwalkerMode(
            string name,
            GlobalKey key,
            TargetDelegate targetDelegate,
            OrbwalkModeDelegate orbwalkBehaviour)
        {
            if (name == null || key == null)
            {
                throw new Exception("There was an error creating the Orbwalker Mode");
            }

            this.Name = name;
            ModeBehaviour = orbwalkBehaviour;
            GetTargetImplementation = targetDelegate;
            this.MenuItem = key.KeyBindItem;
            key.Activate();
            this.UsingGlobalKey = true;
        }

        /// <summary>
        ///     Creates a new instance of an OrbwalkerMode using a new Keybind
        /// </summary>
        public OrbwalkerMode(
            string name,
            KeyCode key,
            TargetDelegate targetDelegate,
            OrbwalkModeDelegate orbwalkBehaviour)
        {
            this.Name = name ?? throw new Exception("There was an error creating the Orbwalker Mode");
            ModeBehaviour = orbwalkBehaviour;
            GetTargetImplementation = targetDelegate;
            this.MenuItem = new MenuKeyBind(name, name, key, KeybindType.Press);
        }

        #endregion

        #region Delegates

        /// <summary>
        ///     The delegate for this Mode's logic
        /// </summary>
        public delegate void OrbwalkModeDelegate();

        /// <summary>
        ///     The delegate for this Mode's target selection logic
        /// </summary>
        public delegate AttackableUnit TargetDelegate();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Whether this mode is currently active
        /// </summary>
        public bool Active => this.MenuItem.Enabled;

        /// <summary>
        ///     Whether attacking is currently allowed
        /// </summary>
        public bool AttackingEnabled { get; set; } = true;

        /// <summary>
        ///     Whether this mode should execute the base Orbwalking Logic
        /// </summary>
        public bool BaseOrbwalkingEnabled { get; set; } = true;

        /// <summary>
        ///     The MenuKeyBind item associated with this mode
        /// </summary>
        public MenuKeyBind MenuItem { get; set; }

        /// <summary>
        ///     Whether moving is currently enabled
        /// </summary>
        public bool MovingEnabled { get; set; } = true;

        /// <summary>
        ///     The name of this mode
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Whether this mode is using a Global Key instead of its own KeyBind
        /// </summary>
        public bool UsingGlobalKey { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Executes the logic for this Orbwalking Mode
        /// </summary>
        public void Execute()
        {
            ModeBehaviour?.Invoke();
        }

        public AttackableUnit GetTarget()
        {
            return GetTargetImplementation?.Invoke();
        }

        #endregion
    }
}