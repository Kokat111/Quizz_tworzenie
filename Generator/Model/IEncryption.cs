﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Model
{
    public interface IEncryption
    {
        string EncryptText(string text);
        string DecryptText(string text);
    }
}
