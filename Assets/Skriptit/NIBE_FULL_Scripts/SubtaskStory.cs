/******************************************************************************
 * File        : SubtaskStory.cs
 * Version     : 1.0
 * Author      : Miika Puljujärvi (miika.puljujarvi@lapinamk.fi)
 * Copyright   : Lapland University of Applied Sciences
 * Licence     : MIT-Licence
 * 
 * Copyright (c) 2022 Lapland University of Applied Sciences
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

using UnityEngine;



/// <summary>
/// Defines which button (if any) is shown when the SubtaskStory is active.
/// </summary>
public enum SubtaskStoryButton { None, Arrow, Rectangular };




/// <summary>
/// SubtaskStory is a collection of textual information and settings for a button. It is always attached to a Subtask's list of SubtaskStories.
/// The contents of a SubtaskStory are displayed in TaskUI. 
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "SubtaskStory", menuName = "SubtaskStory/New SubtaskStory", order = 1)]
public class SubtaskStory : ScriptableObject
{

    // --- --- --- Variables --- --- ---

    public bool showUI = true;
    public bool lookAtPlayerEnabled = true;
    public string headlineFin;
    public string headlineEng;
    public string headlineSwe;
    [TextArea(3, 20)] public string textFin;
    [TextArea(3, 20)] public string textEng;
    [TextArea(3, 20)] public string textSwe;
    public SubtaskStoryButton storyButtonConfiguration;
    public string btnTextFin;
    public string btnTextEng;
    public string btnTextSwe;




    // --- --- --- Methods --- --- ---

    /// <summary>
    /// Return the headline text in the right language.
    /// </summary>
    /// <returns></returns>
    public string GetHeadline (Language _lang)
    {

        switch (_lang) {

            default:
            case Language.Finnish:
                return headlineFin;

            case Language.English:
                return headlineEng;

            case Language.Swedish:
                return headlineSwe;
        }
    }

    /// <summary>
    /// Return the main text in the right language.
    /// </summary>
    /// <returns></returns>
    public string GetText (Language _lang)
    {

        switch (_lang) {

            default:
            case Language.Finnish:
                return textFin;

            case Language.English:
                return textEng;

            case Language.Swedish:
                return textSwe;
        }
    }

    /// <summary>
    /// Return the button text in the right language.
    /// </summary>
    /// <returns></returns>
    public string GetButtonText (Language _lang)
    {
        switch (_lang) {

            default:
            case Language.Finnish:
                return btnTextFin;

            case Language.English:
                return btnTextEng;

            case Language.Swedish:
                return btnTextSwe;
        }
    }
}
