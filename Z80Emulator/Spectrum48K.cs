/*
using IngameScript;
using System;
using System.Resources;

namespace IngameScript
{
    partial class Program
    {
    class Spectrum48K : IComputer
    {
        public Spectrum48K()
        {
            loader = loader ?? new ResourceLoader();
            var clk = new Clock();
            var rom = loader.Spectrum48KROM();
            Array.Resize(ref rom, 0x10000);
            this.Memory = new Memory(rom);
            this.CPU = new CPU(clk);
            this.ULA = new ULA(clk);

            this.CPU.Bind(0xFF, this.ULA.LeakyPort);
            this.CPU.Bind(0xFE, this.ULA);
        }

        public IMemory Memory { get; }
        public CPU CPU { get; }
        public ULA ULA { get; }
    }
}}
*/

