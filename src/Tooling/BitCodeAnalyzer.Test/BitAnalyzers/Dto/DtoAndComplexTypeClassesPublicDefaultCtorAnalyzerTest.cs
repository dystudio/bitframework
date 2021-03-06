﻿using Bit.Tooling.CodeAnalyzer.BitAnalyzers.Dto;
using Bit.Tooling.CodeAnalyzer.Test.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Bit.Tooling.CodeAnalyzer.Test.BitAnalyzers.Dto
{
    [TestClass]
    public class DtoAndComplexTypeClassesPublicDefaultCtorAnalyzerTest : DiagnosticVerifier
    {
        [TestMethod]
        [TestCategory("Analyzer")]
        public async Task FindDtoAndComplexClassPublicDefaultCtor()
        {
            const string sourceCodeWithDtoAndComplexClassPublicDefaultCtor = @"
                public class ClassWithPublicCtorWhichIsNotValidDueHavingParameter : IDto
                {
                   public ClassWithPublicCtorWhichIsNotValidDueHavingParameter(int parameter)
                   {
                   }
                 }

                public class ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined : IDto
                {
                   public ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined()
                   {
                   }

                   internal ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined(int parameter)
                   {
                   }

                   public ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined(string parameter)
                   {
                   }
                 }

                public class ClassWithoutAnyCtorWhichIsValid : IDto
                {
                }

                public class ClassWithCtorWithoutParameterWhichIsNotValid : IDto
                {
                    ClassWithCtorWithoutParameterWhichIsNotValid()
                    {
                    }
                 }

                public class ClassWithCtorWithoutParameterWhichIsNotValid2 : IDto
                {
                    private ClassWithCtorWithoutParameterWhichIsNotValid2()
                    {
                    }
                 }

                public class ClassWithPublicCtorWhichIsNotValidDueHavingParameter2
                {
                   public ClassWithPublicCtorWhichIsNotValidDueHavingParameter2(int parameter)
                   {
                   }
                 }

                public class ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined2
                {
                   public ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined2()
                   {
                   }

                   internal ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined2(int parameter)
                   {
                   }

                   public ClassWithPublicDefaultCtorWhichIsValidEvenWhenThereAreOtherCtorsDefined2(string parameter)
                   {
                   }
                 }

                public class ClassWithoutAnyCtorWhichIsValid2
                {
                }

                public class ClassWithCtorWithoutParameterWhichIsNotValid22
                {
                    ClassWithCtorWithoutParameterWhichIsNotValid22()
                    {
                    }
                 }

                public class ClassWithCtorWithoutParameterWhichIsNotValid22
                {
                    private ClassWithCtorWithoutParameterWhichIsNotValid22()
                    {
                    }
                 }

                public interface IDto {
                }";

            DiagnosticResult[] errors = new DiagnosticResult[] {
                 new DiagnosticResult {
                        Id = nameof(DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer),
                        Message = DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer.Message,
                        Severity = DiagnosticSeverity.Error,
                        Locations = new[] { new DiagnosticResultLocation(2, 17) }
                 },
                    new DiagnosticResult {
                        Id = nameof(DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer),
                        Message = DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer.Message,
                        Severity = DiagnosticSeverity.Error,
                        Locations = new[] { new DiagnosticResultLocation(28, 17) }
                 },
                    new DiagnosticResult {
                        Id = nameof(DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer),
                        Message = DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer.Message,
                        Severity = DiagnosticSeverity.Error,
                        Locations = new[] { new DiagnosticResultLocation(35, 17) }
                 }
            };

            await VerifyCSharpDiagnostic(sourceCodeWithDtoAndComplexClassPublicDefaultCtor, errors);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DtoAndComplexTypeClassesPublicDefaultCtorAnalyzer();
        }
    }
}
