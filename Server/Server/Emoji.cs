using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Emoji
    {
        //Add new emoji here with {Key, Value}
        //If you add a ASCII symbol, like the unicorn, make sure to add a \n at the start of the message
        public readonly Dictionary<string, string> emojiDic = new Dictionary<string, string>
        {
            {"abc", "cde"},
            {"/unicorn", "\n              ,,))))))));,\r\n           __)))))))))))))),\r\n\\|/       -\\(((((''''((((((((.\r\n-*-==//////((''  .     `)))))),\r\n/|\\      ))| o    ;-.    '(((((                                  ,(,\r\n         ( `|    /  )    ;))))'                               ,_))^;(~\r\n            |   |   |   ,))((((_     _____------~~~-.        %,;(;(>';'~\r\n            o_);   ;    )))(((` ~---~  `::           \\      %%~~)(v;(`('~\r\n                  ;    ''''````         `:       `:::|\\,__,%%    );`'; ~\r\n                 |   _                )     /      `:|`----'     `-'\r\n           ______/\\/~    |                 /        /\r\n         /~;;.____/;;'  /          ___--,-(   `;;;/\r\n        / //  _;______;'------~~~~~    /;;/\\    /\r\n       //  | |                        / ;   \\;;,\\\r\n      (<_  | ;                      /',/-----'  _>\r\n       \\_| ||_                     //~;~~~~~~~~~\r\n           `\\_|                   (,~~  \r\n                                   \\~\\\r\n                                    ~~"},
            {"::D:", "😄"},
            {":;):", "😉"},
            {"::o:", "😮"},
            {"::(:", "🙁"},
            {"::/:", "😕"},
            {":<3:", " 💚"},
            {":</3:", "💔"},
            {":100:", "💯"},
            {":ok:", "👌"},
            {":thumbsup:", "👍"}
        };

        /// <summary>
        /// This method searches and replaces key words in a userstring to supported emojis.
        /// </summary>
        /// <param name="input"></param>
        public string ReplaceEmoji(string input)
        {
            foreach (KeyValuePair<string, string> pair in emojiDic)
            {
                input = input.Replace(pair.Key, pair.Value);
            }
            return input;
        }
    }
}
