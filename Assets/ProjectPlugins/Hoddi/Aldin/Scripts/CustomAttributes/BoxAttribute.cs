/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   
 Description    :   
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using System;

[AttributeUsage(AttributeTargets.Field)]
public class BoxGroupAttribute : Attribute {

    public string Header;

    public BoxGroupAttribute(string header)
    {
        this.Header = header;
    }
}
