using UnityEngine.Events;

[System.Serializable]
public class NEvent : UnityEvent {}

[System.Serializable]
public class NIntEvent : UnityEvent<int> {}

[System.Serializable]
public class NFloatEvent : UnityEvent<float> {}

[System.Serializable]
public class NBoolEvent : UnityEvent<bool> {}