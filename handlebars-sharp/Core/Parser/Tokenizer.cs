namespace Handlebars.Core.Parser
{
    using System;
    using System.Collections.Generic;

    public class Tokenizer
    {
        public string Data { get; protected set; }

        public Tokenizer (string data)
        {
            Data = data;
        }

        public IEnumerable <Token> Tokens
        {
            get { 
                int currentPosition = 0;

                while (currentPosition < Data.Length)
                {
                    int position = Data.IndexOf (@"{{", currentPosition);

                    if (position < currentPosition)
                    {
                        // End of string.
                        if (currentPosition < Data.Length)
                        {
                            yield return new Token
                                         {
                                             Type = TokenTypes.StaticText,
                                             Data = Data.Substring (currentPosition)
                                         };
                        }

                        break;
                    }
                    else
                    {
                        // Static data.
                        if (currentPosition != position)
                        {
                            yield return new Token
                                         {
                                             Type = TokenTypes.StaticText,
                                             Data = Data.Substring (currentPosition, position - currentPosition)
                                         };
                        }
                    }

                    if (position >= Data.Length - 2)
                    {
                        throw new Exception (@"Unclosed handlebar expression.");
                    }

                    if (Data [position + 2] == '{')
                    {
                        // Triple mustache.
                        int endPosition = Data.IndexOf (@"}}}", position);
                        if (endPosition == -1)
                        {
                            throw new Exception (@"Unclosed handlebar expression.");
                        }

                        yield return new Token
                                     {
                                         Type = TokenTypes.TripleMustache,
                                         Data = Data.Substring (position + 3, endPosition - position - 3).Trim()
                                     };

                        currentPosition = endPosition + 3;
                    }
                    else
                    {
                        // Double mustache.
                        int endPosition = Data.IndexOf (@"}}", position);
                        if (endPosition == -1)
                        {
                            throw new Exception (@"Unclosed handlebar expression.");
                        }

                        yield return new Token
                                     {
                                         Type = TokenTypes.DoubleMustache,
                                         Data = Data.Substring (position + 2, endPosition - position - 2).Trim()
                                     };

                        currentPosition = endPosition + 2;
                    }
                }
            }
        }
    }
}