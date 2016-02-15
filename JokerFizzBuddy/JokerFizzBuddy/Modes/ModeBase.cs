using EloBuddy.SDK;

namespace JokerFizzBuddy.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Targeted Q
        {
            get { return SpellManager.Q; }
        }

        protected static Spell.Active W
        {
            get { return SpellManager.W; }
        }

        protected static Spell.Skillshot E
        {
            get { return SpellManager.E; }
        }

        protected static Spell.Skillshot R
        {
            get { return SpellManager.R; }
        }

        protected static Spell.Skillshot FLASH
        {
            get { return SpellManager.FLASH; }
        }

        protected static Spell.Targeted IG
        {
            get { return SpellManager.IG; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }

}
