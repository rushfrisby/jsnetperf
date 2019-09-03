using BenchmarkDotNet.Attributes;

namespace JsEnginePerformanceComparison
{
    public class SunspiderBenchmark : EngineBenchmark
    {
        static SunspiderBenchmark()
        {
            AddTest(5, @"sunspider\sunspider-3d-morph.js");
            AddTest(5, @"sunspider\sunspider-3d-raytrace.js");
            AddTest(5, @"sunspider\sunspider-access-binary-trees.js");
            AddTest(5, @"sunspider\sunspider-access-fannkuch.js");
            AddTest(5, @"sunspider\sunspider-access-nbody.js");
            AddTest(5, @"sunspider\sunspider-access-nsieve.js");
            AddTest(5, @"sunspider\sunspider-bitops-3bit-bits-in-byte.js");
            AddTest(5, @"sunspider\sunspider-bitops-bits-in-byte.js");
            AddTest(5, @"sunspider\sunspider-bitops-bitwise-and.js");
            AddTest(5, @"sunspider\sunspider-bitops-nsieve-bits.js");
            AddTest(10, @"sunspider\sunspider-controlflow-recursive.js");
            AddTest(10, @"sunspider\sunspider-crypto-aes.js");
            AddTest(5, @"sunspider\sunspider-crypto-md5.js");
            AddTest(5, @"sunspider\sunspider-crypto-sha1.js");
            AddTest(5, @"sunspider\sunspider-date-format-tofte.js");
            AddTest(5, @"sunspider\sunspider-date-format-xparb.js");
            AddTest(5, @"sunspider\sunspider-math-cordic.js");
            AddTest(5, @"sunspider\sunspider-math-partial-sums.js");
            AddTest(5, @"sunspider\sunspider-math-spectral-norm.js");
            AddTest(5, @"sunspider\sunspider-regexp-dna.js");
            AddTest(5, @"sunspider\sunspider-string-fasta.js");
            AddTest(10, @"sunspider\sunspider-string-tagcloud.js");
            AddTest(5, @"sunspider\sunspider-string-unpack-code.js");
            AddTest(5, @"sunspider\sunspider-string-validate-input.js");
        }

        [Benchmark(Description = "sunspider-3d-morph.js")]
        public void Morph3D()
        {
            Run("sunspider-3d-morph.js");
        }

        [Benchmark(Description = "sunspider-3d-raytrace.js")]
        public void Raytrace3D()
        {
            Run("sunspider-3d-raytrace.js");
        }

        [Benchmark(Description = "sunspider-access-binary-trees.js")]
        public void AccessBinaryTrees()
        {
            Run("sunspider-access-binary-trees.js");
        }

        [Benchmark(Description = "sunspider-access-fannkuch.js")]
        public void AccessFannkuch()
        {
            Run("sunspider-access-fannkuch.js");
        }

        [Benchmark(Description = "sunspider-access-nbody.js")]
        public void AccessNbody()
        {
            Run("sunspider-access-nbody.js");
        }

        [Benchmark(Description = "sunspider-access-nsieve.js")]
        public void AccessNsieve()
        {
            Run("sunspider-access-nsieve.js");
        }

        [Benchmark(Description = "sunspider-bitops-3bit-bits-in-byte.js")]
        public void Bitops3BitBitsInByte()
        {
            Run("sunspider-bitops-3bit-bits-in-byte.js");
        }

        [Benchmark(Description = "sunspider-bitops-bits-in-byte.js")]
        public void BitopsBitsInByte()
        {
            Run("sunspider-bitops-bits-in-byte.js");
        }

        [Benchmark(Description = "sunspider-bitops-bitwise-and.js")]
        public void BitopsBitwiseAnd()
        {
            Run("sunspider-bitops-bitwise-and.js");
        }

        [Benchmark(Description = "sunspider-bitops-nsieve-bits.js")]
        public void BitopsNsieveBits()
        {
            Run("sunspider-bitops-nsieve-bits.js");
        }

        [Benchmark(Description = "sunspider-controlflow-recursive.js")]
        public void ControlflowRecursive()
        {
            Run("sunspider-controlflow-recursive.js");
        }

        [Benchmark(Description = "sunspider-crypto-aes.js")]
        public void CryptoAes()
        {
            Run("sunspider-crypto-aes.js");
        }

        [Benchmark(Description = "sunspider-crypto-md5.js")]
        public void CryptoMd5()
        {
            Run("sunspider-crypto-md5.js");
        }

        [Benchmark(Description = "sunspider-crypto-sha1.js")]
        public void CryptoSha1()
        {
            Run("sunspider-crypto-sha1.js");
        }

        [Benchmark(Description = "sunspider-date-format-tofte.js")]
        public void DateFormatTofte()
        {
            Run("sunspider-date-format-tofte.js");
        }

        [Benchmark(Description = "sunspider-date-format-xparb.js")]
        public void DateFormatXparb()
        {
            Run("sunspider-date-format-xparb.js");
        }

        [Benchmark(Description = "sunspider-math-cordic.js")]
        public void MathCordic()
        {
            Run("sunspider-math-cordic.js");
        }

        [Benchmark(Description = "sunspider-math-partial-sums.js")]
        public void MathPartialSums()
        {
            Run("sunspider-math-partial-sums.js");
        }

        [Benchmark(Description = "sunspider-math-spectral-norm.js")]
        public void MathSpectralNorm()
        {
            Run("sunspider-math-spectral-norm.js");
        }

        [Benchmark(Description = "sunspider-regexp-dna.js")]
        public void RegexpDna()
        {
            Run("sunspider-regexp-dna.js");
        }

        [Benchmark(Description = "sunspider-string-fasta.js")]
        public void StringFasta()
        {
            Run("sunspider-string-fasta.js");
        }

        [Benchmark(Description = "sunspider-string-tagcloud.js")]
        public void StringTagcloud()
        {
            Run("sunspider-string-tagcloud.js");
        }

        [Benchmark(Description = "sunspider-string-unpack-code.js")]
        public void StringUnpackCode()
        {
            Run("sunspider-string-unpack-code.js");
        }

        [Benchmark(Description = "sunspider-string-validate-input.js")]
        public void StringValidateInput()
        {
            Run("sunspider-string-validate-input.js");
        }
    }
}