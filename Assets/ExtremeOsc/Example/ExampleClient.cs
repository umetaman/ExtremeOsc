using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeOsc;
using ExtremeOsc.Annotations;

namespace ExtremeOsc.Example
{
    [OscPackable]
    public partial class ExampleData
    {
        [OscElementAt(0)]
        public int IntValue { get; set; }
        [OscElementAt(1)]
        public float FloatValue { get; set; }
        [OscElementAt(2)]
        public string StringValue { get; set; }

        public ExampleData()
        {
            IntValue = 0;
            FloatValue = 0.0f;
            StringValue = string.Empty;
        }
    }

    public class ExampleClient : MonoBehaviour
    {
        private OscClient client = null;

        private void Awake()
        {
            client = new OscClient("127.0.0.1", 5555);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                var data = new ExampleData
                {
                    IntValue = Random.Range(0, 100),
                    FloatValue = Random.Range(0.0f, 1.0f),
                    StringValue = "Hello, World!"
                };
                Debug.Log("Send: " + data.IntValue + ", " + data.FloatValue + ", " + data.StringValue);
                client.Send("/example", data);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                var data = new ExampleData
                {
                    IntValue = Random.Range(0, 100),
                    FloatValue = Random.Range(0.0f, 1.0f),
                    StringValue = "Hello, World!"
                };
                Debug.Log("Send: " + data.IntValue + ", " + data.FloatValue + ", " + data.StringValue);
                client.Send("/example/arguments", data);
            }
        }

        private void OnDestroy()
        {
            client?.Dispose();
            client = null;
        }
    }
}
