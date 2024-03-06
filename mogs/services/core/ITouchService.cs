using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITouchService
{
    void OnTouch(IInputService input);
    void OnRelease(IInputService input);
}
