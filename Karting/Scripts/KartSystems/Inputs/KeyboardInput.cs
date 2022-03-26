using UnityEngine;

namespace KartGame.KartSystems {

    public class KeyboardInput : BaseInput
    {
        public string TurnInputName = "Horizontal";
        public string AccelerateButtonName = "Accelerate";
        public string BrakeButtonName = "Brake";

        public float TurnInput = 0;
        public bool Accelerate = false;
        public bool Brake = false;
        public override InputData GenerateInput() {

#if UNITY_EDITOR
            return new InputData
            {
                Accelerate = Input.GetButton(AccelerateButtonName),
                Brake = Input.GetButton(BrakeButtonName),
                TurnInput = Input.GetAxis("Horizontal")
            };

#else
     return new InputData
            {
                Accelerate = this.Accelerate,
                Brake = this.Brake,
                TurnInput=this.TurnInput
            };
           
#endif

        }
        public void SetAngle(float angle)
        {
            TurnInput = angle;
        }
        public void SetAccelaration(bool acc)
        {
            Accelerate = acc;
        }
        public void SetBrake(bool brake)
        {
            Brake = brake;
        }
       
    }
}
