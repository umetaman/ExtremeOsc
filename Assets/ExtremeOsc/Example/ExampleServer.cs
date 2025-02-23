using ExtremeOsc.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeOsc.Example
{
    [OscReceiver]
    public partial class ExampleServer : MonoBehaviour
    {
        private OscServer server = null;

        [OscCallback("/example")]
        private void OnExample(string address, ExampleData data)
        {
            Debug.Log($"{address} => IntValue: {data.IntValue}, FloatValue: {data.FloatValue}, StringValue: {data.StringValue}");
        }

        [OscCallback("/example/arguments")]
        private void OnExampleArguments(string address, int intValue, float floatValue, string stringValue)
        {
            Debug.Log($"{address} => IntValue: {intValue}, FloatValue: {floatValue}, StringValue: {stringValue}");
        }

        private void Awake()
        {
            server = new OscServer(5555);
            server.Register(this);
            server.Open();
        }

        private void OnDestroy()
        {
            server.Unregister(this);
            server?.Dispose();
            server = null;
        }
    }
}
