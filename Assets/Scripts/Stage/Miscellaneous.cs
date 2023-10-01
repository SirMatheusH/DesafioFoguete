using UnityEngine;

namespace Stage
{
    /**
     * Funções criadas para simplificar, abstrair, e diminuir repetições de código.
     */
    public static class Miscellaneous
    {
        public static void AddUpwardsForce(this Rigidbody receiver, GameObject gameObject, float force)
        {
            receiver.AddForce(gameObject.transform.forward * force, ForceMode.Force); // transform.forward e não transform.up devido à orientação do nariz do foguete.
        }
        
        public static void AddRotationalTorque(this Rigidbody receiver, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    receiver.AddRelativeTorque(new Vector3(0, -1, 0) * 5, ForceMode.Acceleration);
                    break;
                case Direction.Right:
                    receiver.AddRelativeTorque(new Vector3(0, 1, 0) * 5, ForceMode.Acceleration);
                    break;
                case Direction.Forwards:
                    receiver.AddRelativeTorque(new Vector3(1, 0, 0) * 5, ForceMode.Acceleration);
                    break;
                case Direction.Backwards:
                    receiver.AddRelativeTorque(new Vector3(-1, 1, 0) * 5, ForceMode.Acceleration);
                    break;
                default:
                    return;
            }
        }

        /**
         * Calcula a porcentagem que um número é de outro.
         */
        public static int PercentageBetweenTwoValues(float value, float maxValue)
        {
            return (int)((value / maxValue) * 100);
        }
    }
    
    /**
     * As direções são relativas à orientação fixa da câmera
     */
    public enum Direction
    {
        Right,
        Left,
        Forwards,
        Backwards
    }
}

