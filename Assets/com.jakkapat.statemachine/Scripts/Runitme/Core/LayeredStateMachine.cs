using System.Collections.Generic;

namespace Jakkapat.ToppuFSM.Core
{
    public class LayeredStateMachine<TContext>
    {
        private Dictionary<string, StateMachine<TContext>> layers = new Dictionary<string, StateMachine<TContext>>();

        /// <summary>
        /// Adds or replaces an entire state machine layer.
        /// </summary>
        public void SetLayerStateMachine(string layerName, StateMachine<TContext> stateMachine)
        {
            layers[layerName] = stateMachine; // Overwrite or add
        }

        /// <summary>
        /// Initialize a specific layer with a starting state.
        /// </summary>
        public void InitializeLayer(string layerName, TContext context, IState<TContext> startingState)
        {
            // If no layer exists, create a new one
            if (!layers.ContainsKey(layerName))
            {
                var newLayer = new StateMachine<TContext>(context, startingState);
                layers.Add(layerName, newLayer);
            }
            else
            {
                layers[layerName].Initialize(startingState);
            }
        }

        /// <summary>
        /// Update all layers each frame.
        /// </summary>
        public void UpdateAllLayers()
        {
            foreach (var layerPair in layers)
            {
                layerPair.Value.Update();
            }
        }

        /// <summary>
        /// Change state in a specific layer.
        /// </summary>
        public void ChangeLayerState(string layerName, IState<TContext> newState)
        {
            if (layers.ContainsKey(layerName))
            {
                layers[layerName].ChangeState(newState);
            }
        }

        /// <summary>
        /// Retrieve the current state of a layer.
        /// </summary>
        public IState<TContext> GetCurrentState(string layerName)
        {
            if (layers.ContainsKey(layerName))
            {
                return layers[layerName].CurrentState;
            }
            return null;
        }
    }
}
