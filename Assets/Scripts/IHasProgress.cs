using System;
public interface IHasProgress {

    public event EventHandler<OnProgresChangedEventArgs> OnProgressChanged;
    public class OnProgresChangedEventArgs : EventArgs {
        public float progressNormalized;
    }
}
