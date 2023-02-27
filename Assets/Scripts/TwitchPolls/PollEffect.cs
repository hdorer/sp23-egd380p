using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PollEffect {
    protected float duration;

    public PollEffect(float duration) {
        this.duration = duration;
    }

    public abstract void onActivate();
    public abstract void onDeactivate();

    public IEnumerator doEffect() {
        onActivate();
        yield return new WaitForSeconds(duration);
        onDeactivate();
    }
}
