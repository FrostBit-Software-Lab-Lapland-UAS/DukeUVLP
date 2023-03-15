/******************************************************************************
 * File        : GameEvents.cs
 * Version     : 1.0
 * Author      : Severi Kangas (severi.kangas@lapinamk.com)
 * Copyright   : Lapland University of Applied Sciences
 * Licence     : MIT-Licence
 * 
 * Copyright (c) 2021 Lapland University of Applied Sciences
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 *****************************************************************************/

using System;
using UnityEngine;

public static class GameEvents 
{

    public static event Action onFirstEvent;
    public static void FirstEvent()
    {
        if(onFirstEvent != null)
        {
            onFirstEvent.Invoke();
        }
    }
  
    public static event Action onSecondEvent;
    public static void SecondEvent()
    {
        if(onSecondEvent != null)
        {
            onSecondEvent.Invoke();
        }
    }

    // PetteriM: from pressure change to next event -> handle turn
    
    public static event Action onThirdEvent;
    public static void ThirdEvent()
    {
        if(onThirdEvent != null)
        {
            onThirdEvent.Invoke();
        }
    }

    // PetteriM: handle is turned here
   
    public static event Action onFourthEvent;
    public static void FourthEvent()
    {
        if(onFourthEvent != null)
        {
            onFourthEvent.Invoke();
        }
    }

    // PetteriM: mutasihti washing event
    public static event Action onFifthEvent;
    public static void FifthEvent()
    {
        if(onFifthEvent != null)
        {
            onFifthEvent.Invoke();
        }
    }

    public static event Action onSixthEvent;
    public static void SixthEvent()
    {
       
        if (onSixthEvent != null)
        {
            onSixthEvent.Invoke();
        }
    }

    public static event Action onSeventhEvent;
    public static void SeventhEvent()
    {
        if(onSeventhEvent != null)
        {
            onSeventhEvent.Invoke();
        }
    }

    public static event Action onEighthEvent;
    public static void EighthEvent()
    {
        if (onEighthEvent != null)
        {
            onEighthEvent.Invoke();
        }
    }

}
