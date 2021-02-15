using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;
using Moq;

namespace Tests
{
    public class AttributeSetsContainer
    {
        public AttributeSetsContainer()
        {
            AttributeSets = new List<AttributeSet>();
            Containers = new List<AttributeSetContainer>();
        }

        public string Name { get; set; }

        public List<AttributeSet> AttributeSets { get; set; }
        public List<AttributeSetContainer> Containers { get; set; }
    }

    public class AttributeSetContainer
    {
        public AttributeSetContainer()
        {
            Attributes = new List<Inventor.Attribute>();
        }

        public string Name { get; set; }

        public List<Inventor.Attribute> Attributes { get; set; }
    }

    public static class MockFactory
    {
        public static Inventor.Attribute CreateAttribute(string name, ValueTypeEnum typ, object value, AttributeSet parent)
        {
            var attMock = new Mock<Inventor.Attribute>();

            attMock.Setup(x => x.Value).Returns(value);
            attMock.Setup(x => x.Name).Returns(name);
            attMock.Setup(x => x.Value).Returns(typ);
            attMock.Setup(x => x.Parent).Returns(parent);
            return attMock.Object;
        }

        public static Mock<AttributeSets> CreateAttributeSets(AttributeSetsContainer container)
        {
            var attSetsMock = new Mock<AttributeSets>();
            attSetsMock.Setup(x => x.Count).Returns(() => { return container.AttributeSets.Count; });
            attSetsMock.Setup(m => m[It.IsAny<int>()]).Returns<int>(i => container.AttributeSets.ElementAt(i));
            attSetsMock.As<IEnumerable>().Setup(m => m.GetEnumerator()).Returns(() => container.AttributeSets.GetEnumerator());
            attSetsMock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, bool>(
                    (s, b) =>
                    {
                        var c = new AttributeSetContainer() { Name = s };
                        var attributeSet = CreateAttributeSet(c, attSetsMock.Object);
                        container.Containers.Add(c);
                        container.AttributeSets.Add(attributeSet.Object);

                        return attributeSet.Object;
                    });
            attSetsMock.Setup(x => x.get_NameIsUsed(It.IsAny<string>())).Returns<string>(r => container.AttributeSets.Any(y => y.Name == r));

            return attSetsMock;
        }

        public static Mock<AttributeSet> CreateAttributeSet(AttributeSetContainer container, AttributeSets parent)
        {
            var attMock = new Mock<AttributeSet>();
            attMock.Setup(x => x.Name).Returns(container.Name);
            attMock.As<AttributeSet>().Setup(x => x.Count).Returns(() =>
            {
                return container.Attributes.Count;
            });
            attMock.Setup(m => m[It.IsAny<int>()]).Returns<int>(i => container.Attributes.ElementAt(i).Value);
            attMock.As<IEnumerable>().Setup(m => m.GetEnumerator()).Returns(() => container.Attributes.GetEnumerator());
            attMock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<ValueTypeEnum>(), It.IsAny<object>()))
                .Returns<string, ValueTypeEnum, object>(
                    (s, v, o) =>
                    {
                        var item = MockFactory.CreateAttribute(s, v, o, attMock.Object);
                        container.Attributes.Add(item);
                        return item;
                    });
            attMock.Setup(x => x.get_NameIsUsed(It.IsAny<string>())).Returns<string>(r => container.Attributes.Any(y => y.Name == r));
            attMock.Setup(x => x.Parent).Returns(parent);
            return attMock;
        }
    }
}

