using System.Reflection;
using NUnit.Framework;

namespace RgbConverter.Tests
{
    public class RgbColorTests
    {
        private static readonly object[][] ConstructorData =
        {
            new object[]
            {
                new[] { typeof(byte), typeof(byte), typeof(byte) },
            },
        };

        private static readonly object[][] HasMethodData =
        {
            new object[]
            {
                "Equals", false, true, true, typeof(bool), new Type[] { typeof(RgbColor) },
            },
            new object[]
            {
                "Equals", false, true, true, typeof(bool), new Type[] { typeof(object) },
            },
            new object[]
            {
                "ToString", false, true, true, typeof(string), Array.Empty<Type>(),
            },
            new object[]
            {
                "GetHashCode", false, true, true, typeof(int), Array.Empty<Type>(),
            },
            new object[]
            {
                "Parse", true, true, false, typeof(RgbColor), new Type[] { typeof(string) },
            },
            new object[]
            {
                "TryParse", true, true, false, typeof(bool), new Type[] { typeof(string), typeof(RgbColor).MakeByRefType() },
            },
            new object[]
            {
                "Create", true, true, false, typeof(RgbColor), new Type[] { typeof(int), typeof(int), typeof(int) },
            },
            new object[]
            {
                "Create", true, true, false, typeof(RgbColor), new Type[] { typeof(long), typeof(long), typeof(long) },
            },
            new object[]
            {
                "ThrowExceptionIfValueIsNotValid", true, false, false, typeof(void), new Type[] { typeof(long), typeof(string) },
            },
            new object[]
            {
                "op_Equality", true, true, false, typeof(bool), new Type[] { typeof(RgbColor), typeof(RgbColor) },
            },
            new object[]
            {
                "op_Inequality", true, true, false, typeof(bool), new Type[] { typeof(RgbColor), typeof(RgbColor) },
            },
        };

        private static readonly object[][] EqualsWithRgbColorParameterData =
        {
            new object[] { new RgbColor(0, 0, 0), new RgbColor(0, 0, 0), true },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(255, 255, 255), true },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(0, 255, 255), false },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(255, 0, 255), false },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(255, 255, 0), false },
        };

        private static readonly object?[][] EqualsWithObjectParameterData =
        {
            new object[] { new RgbColor(0, 0, 0), new RgbColor(0, 0, 0), true },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(255, 255, 255), true },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(0, 255, 255), false },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(255, 0, 255), false },
            new object[] { new RgbColor(255, 255, 255), new RgbColor(255, 255, 0), false },
            new object?[] { new RgbColor(255, 255, 255), null, false },
            new object[] { new RgbColor(255, 255, 255), new object(), false },
        };

        private Type? classType;

        [SetUp]
        public void SetUp()
        {
            this.classType = typeof(RgbColor);
        }

        [TestCase(0x00, 0x00, 0x00)]
        [TestCase(0x01, 0x02, 0x03)]
        [TestCase(0x0F, 0xF0, 0xFF)]
        public void RgbColor_ReturnsNewObject(byte red, byte green, byte blue)
        {
            // Act
            var rgbColor = new RgbColor(red: red, green: green, blue: blue);

            // Assert
            Assert.That(rgbColor.Red, Is.EqualTo(red));
            Assert.That(rgbColor.Green, Is.EqualTo(green));
            Assert.That(rgbColor.Blue, Is.EqualTo(blue));
        }

        [TestCase("")]
        [TestCase("      ")]
        [TestCase("AGCDEF")]
        [TestCase("ABCGEF")]
        [TestCase("ABCDEG")]
        public void Parse_ThrowsArgumentException(string rgbString)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                try
                {
                    // Act
                    RgbColor.Parse(rgbString);
                }
                catch (ArgumentException e)
                {
                    Assert.That(e.ParamName, Is.EqualTo(nameof(rgbString)));
                    throw;
                }
            });
        }

        [TestCase("000000", 0x00, 0x00, 0x00)]
        [TestCase("010203", 0x01, 0x02, 0x03)]
        [TestCase("ABCDEF", 0xAB, 0xCD, 0xEF)]
        [TestCase("abcdef", 0xAB, 0xCD, 0xEF)]
        [TestCase("FFFFFF", 0xFF, 0xFF, 0xFF)]
        [TestCase("ffffff", 0xFF, 0xFF, 0xFF)]
        public void Parse_ReturnsRgbColor(string rgbString, byte red, byte green, byte blue)
        {
            // Act
            RgbColor rgbColor = RgbColor.Parse(rgbString: rgbString);

            // Assert
            Assert.That(rgbColor.Red, Is.EqualTo(red));
            Assert.That(rgbColor.Green, Is.EqualTo(green));
            Assert.That(rgbColor.Blue, Is.EqualTo(blue));
        }

        [TestCase("", false, 0, 0, 0)]
        [TestCase("      ", false, 0, 0, 0)]
        [TestCase("AGCDEF", false, 0, 0, 0)]
        [TestCase("ABCGEF", false, 0, 0, 0)]
        [TestCase("ABCDEG", false, 0, 0, 0)]
        [TestCase("000000", true, 0x00, 0x00, 0x00)]
        [TestCase("010203", true, 0x01, 0x02, 0x03)]
        [TestCase("ABCDEF", true, 0xAB, 0xCD, 0xEF)]
        [TestCase("abcdef", true, 0xAB, 0xCD, 0xEF)]
        [TestCase("FFFFFF", true, 0xFF, 0xFF, 0xFF)]
        [TestCase("ffffff", true, 0xFF, 0xFF, 0xFF)]
        public void TryParse_ReturnsRgbColor(string rgbString, bool expectedResult, byte red, byte green, byte blue)
        {
            // Act
            bool actualResult = RgbColor.TryParse(rgbString: rgbString, rgbColor: out RgbColor rgbColor);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
            Assert.That(rgbColor.Red, Is.EqualTo(red));
            Assert.That(rgbColor.Green, Is.EqualTo(green));
            Assert.That(rgbColor.Blue, Is.EqualTo(blue));
        }

        [TestCase(-1, 0, 0, "red")]
        [TestCase(256, 0, 0, "red")]
        [TestCase(0, -1, 0, "green")]
        [TestCase(0, 256, 0, "green")]
        [TestCase(0, 0, -1, "blue")]
        [TestCase(0, 0, 256, "blue")]
        public void Create_WithIntParameters_ThrowsArgumentException(int red, int green, int blue, string parameterName)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                try
                {
                    // Act
                    _ = RgbColor.Create(red: red, green: green, blue: blue);
                }
                catch (ArgumentException e)
                {
                    Assert.That(e.ParamName, Is.EqualTo(parameterName));
                    throw;
                }
            });
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 3)]
        [TestCase(100, 150, 200)]
        [TestCase(255, 255, 255)]
        public void Create_WithIntParameters_ReturnsNewObject(int red, int green, int blue)
        {
            // Act
            var rgbColor = RgbColor.Create(red: red, green: green, blue: blue);

            // Assert
            Assert.That(rgbColor.Red, Is.EqualTo(red));
            Assert.That(rgbColor.Green, Is.EqualTo(green));
            Assert.That(rgbColor.Blue, Is.EqualTo(blue));
        }

        [TestCase(-1L, 0L, 0L, "red")]
        [TestCase(256L, 0L, 0L, "red")]
        [TestCase(0L, -1L, 0L, "green")]
        [TestCase(0L, 256L, 0L, "green")]
        [TestCase(0L, 0L, -1L, "blue")]
        [TestCase(0L, 0L, 256L, "blue")]
        public void Create_WithLongParameters_ThrowsArgumentException(long red, long green, long blue, string parameterName)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                try
                {
                    // Act
                    _ = RgbColor.Create(red: red, green: green, blue: blue);
                }
                catch (ArgumentException e)
                {
                    Assert.That(e.ParamName, Is.EqualTo(parameterName));
                    throw;
                }
            });
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 3)]
        [TestCase(100, 150, 200)]
        [TestCase(255, 255, 255)]
        public void Create_WithLongParameters_ReturnsNewObject(long red, long green, long blue)
        {
            // Act
            var rgbColor = RgbColor.Create(red: red, green: green, blue: blue);

            // Assert
            Assert.That(rgbColor.Red, Is.EqualTo(red));
            Assert.That(rgbColor.Green, Is.EqualTo(green));
            Assert.That(rgbColor.Blue, Is.EqualTo(blue));
        }

        [TestCaseSource(nameof(EqualsWithRgbColorParameterData))]
        public void Equals_WithRgbColorParameter_ReturnsBoolean(RgbColor rgbColor, RgbColor other, bool expectedResult)
        {
            // Act
            bool actualResult = rgbColor.Equals(other: other);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(EqualsWithObjectParameterData))]
        public void Equals_WithObjectParameter_ReturnsBoolean(object rgbColor, object? obj, bool expectedResult)
        {
            // Act
            bool actualResult = rgbColor.Equals(obj: obj);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(nameof(EqualsWithRgbColorParameterData))]
        public void EqualityOperators_ReturnsBoolean(RgbColor left, RgbColor right, bool expectedResult)
        {
            // Act
            bool isEqual = left == right;
            bool isUnequal = left != right;

            // Assert
            Assert.That(isEqual, Is.EqualTo(expectedResult));
            Assert.That(isUnequal, Is.EqualTo(!expectedResult));
        }

        [TestCase(0, 0, 0, ExpectedResult = "000000")]
        [TestCase(0xAB, 0xCD, 0xEF, ExpectedResult = "ABCDEF")]
        [TestCase(0xFF, 0xFF, 0xFF, ExpectedResult = "FFFFFF")]
        public string ToString_ReturnsString(byte red, byte green, byte blue)
        {
            // Arrange
            var rgbColor = new RgbColor(red: red, green: green, blue: blue);

            // Act
            return rgbColor.ToString();
        }

        [TestCase(0x00, 0x00, 0x00, ExpectedResult = 0x000000)]
        [TestCase(0x01, 0x02, 0x03, ExpectedResult = 0x030201)]
        [TestCase(0xAB, 0xCD, 0xEF, ExpectedResult = 0xEFCDAB)]
        [TestCase(0xFF, 0xFF, 0xFF, ExpectedResult = 0xFFFFFF)]
        public int GetHashCode_ReturnsHashCode(byte red, byte green, byte blue)
        {
            // Arrange
            var rgbColor = new RgbColor(red: red, green: green, blue: blue);

            // Act
            return rgbColor.GetHashCode();
        }

        [Test]
        public void IsPublicStruct()
        {
            this.AssertThatStructIsPublic();
        }

        [Test]
        public void InheritsValuteType()
        {
            this.AssertThatStructInheritsValueType();
        }

        [Test]
        public void ImplementsEquitable()
        {
            this.AssertThatTypeImplementsInterface(typeof(IEquatable<RgbColor>));
        }

        [Test]
        public void HasRequiredMembers()
        {
            Assert.AreEqual(0, this.classType!.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(0, this.classType!.GetFields(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(3, this.classType!.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.classType!.GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(1, this.classType!.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.classType!.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.classType!.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(3, this.classType!.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.classType!.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(6, this.classType!.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(1, this.classType!.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(7, this.classType!.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(0, this.classType!.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(0, this.classType!.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length);
        }

        [TestCase("red", typeof(byte), true)]
        [TestCase("green", typeof(byte), true)]
        [TestCase("blue", typeof(byte), true)]
        public void HasRequiredField(string fieldName, Type fieldType, bool isInitOnly)
        {
            this.AssertThatTypeHasField(fieldName, fieldType, isInitOnly);
        }

        [TestCaseSource(nameof(ConstructorData))]
        public void HasPublicInstanceConstructor(Type[] parameterTypes)
        {
            this.AssertThatTypeHasPublicConstructor(parameterTypes);
        }

        [TestCase("Red", typeof(byte), true, true, false, false)]
        [TestCase("Green", typeof(byte), true, true, false, false)]
        [TestCase("Blue", typeof(byte), true, true, false, false)]
        public void HasProperty(string propertyName, Type propertyType, bool hasGet, bool isGetPublic, bool hasSet, bool isSetPublic)
        {
            this.AssertThatTypeHasProperty(propertyName, propertyType, hasGet, isGetPublic, hasSet, isSetPublic);
        }

        [TestCaseSource(nameof(HasMethodData))]
        public void HasMethod(string methodName, bool isStatic, bool isPublic, bool isVirtual, Type returnType, Type[] parameters)
        {
            this.AssertThatTypeHasMethod(methodName, isStatic, isPublic, isVirtual, returnType, parameters);
        }

        private void AssertThatStructIsPublic()
        {
            Assert.That(this.classType!.IsValueType, Is.True);
            Assert.That(this.classType!.IsPublic, Is.True);
            Assert.That(this.classType!.IsAbstract, Is.False);
        }

        private void AssertThatTypeHasField(string fieldName, Type fieldType, bool isInitOnly)
        {
            var fieldInfo = this.classType?.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.That(fieldInfo, Is.Not.Null);
            Assert.That(fieldInfo!.FieldType, Is.EqualTo(fieldType));
            Assert.That(fieldInfo!.IsInitOnly, isInitOnly ? Is.True : Is.False);
        }

        private void AssertThatStructInheritsValueType()
        {
            Assert.That(this.classType!.BaseType, Is.EqualTo(typeof(ValueType)));
        }

        private void AssertThatTypeHasPublicConstructor(Type[] parameterTypes)
        {
            var constructorInfo = this.classType!.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null);
            Assert.That(constructorInfo, Is.Not.Null);
        }

        private void AssertThatTypeHasProperty(string propertyName, Type expectedPropertyType, bool hasGet, bool isGetPublic, bool hasSet, bool isSetPublic)
        {
            var propertyInfo = this.classType!.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);

            Assert.That(propertyInfo, Is.Not.Null);
            Assert.That(propertyInfo!.PropertyType, Is.EqualTo(expectedPropertyType));

            if (hasGet)
            {
                Assert.That(propertyInfo!.GetMethod!.IsPublic, isGetPublic ? Is.True : Is.False);
            }

            if (hasSet)
            {
                Assert.That(propertyInfo!.SetMethod!.IsPublic, isSetPublic ? Is.True : Is.False);
            }
        }

        private void AssertThatTypeHasMethod(string methodName, bool isStatic, bool isPublic, bool isVirtual, Type returnType, Type[] parameters)
        {
            var methodInfo = this.classType!.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, parameters);

            Assert.That(methodInfo, Is.Not.Null);
            Assert.That(methodInfo!.IsStatic, isStatic ? Is.True : Is.False);
            Assert.That(methodInfo!.IsPublic, isPublic ? Is.True : Is.False);
            Assert.That(methodInfo!.IsVirtual, isVirtual ? Is.True : Is.False);
            Assert.That(methodInfo!.ReturnType, Is.EqualTo(returnType));
        }

        private void AssertThatTypeImplementsInterface(Type interfaceType)
        {
            var @interface = this.classType!.GetInterface(interfaceType.Name);

            Assert.That(@interface, Is.Not.Null);
        }
    }
}
