using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),


namespace DMT
{
    public class DMTMQTTSenderSmall : M2MqttUnityClient
    {
        private List<string> eventMessages = new List<string>();
        private int txCnt = 0;
        private int rxCnt = 0;

        private const string topicTest = "unity/helloworld";
        private const string topicSubscribe = "unity/helloworld";

        protected override void Start()
        {
            Debug.Log("MQTT small is ready.");

            brokerPort = 1883;
            brokerAddress = "dmt.fh-joanneum.at";
            mqttUserName = "dmt";
            mqttPassword = "ss2021";
            isEncrypted = false;
            autoConnect = true;

            Invoke("TestPublish", 5);
            base.Start();
        }

        public void TestPublish()
        {
            string sendMsg = "welcome";
            // client.Publish(topicTest, System.Text.Encoding.UTF8.GetBytes(sendMsg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            // Debug.Log("##### Test message published: " + topicTest + " " + sendMsg);
            SendPublish(topicTest, sendMsg);
        }

        public void SendPublish(string topic, string sendMsg)
        {
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(sendMsg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("##### MQTT SendPublish: " + topic + " " + sendMsg + " [" + (++txCnt) + "]");
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { topicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {

            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("MQTT Received: " + msg + " - " + (++rxCnt));
        }

        private void OnDestroy()
        {
            Disconnect();
        }

    }
}