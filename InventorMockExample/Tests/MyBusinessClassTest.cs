using FluentAssertions;
using Inventor;
using InventorMockExample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class MyBusinessClassTest
    {
        [TestMethod]
        public void property_does_not_exists_save_should_create_property()
        {
            //arange

            string value = Guid.NewGuid().ToString();
            string id = "12345";

            var container = new AttributeSetsContainer();
            var attsetsMock = MockFactory.CreateAttributeSets(container);

            var applicationMock = new Mock<IApplicationService>();

            applicationMock.Setup(x => x.GetCurrentDocumentAttributeSets()).Returns(attsetsMock.Object);

            var businessClass = new MyBusinessClass(applicationMock.Object);

            //act

            businessClass.SaveMyProperty(value, id);

            //assert

            container.AttributeSets.Count.Should().Be(1);
            container.AttributeSets.Single(x => x.Name == "one").Should().NotBeNull();
            var containerAttributeSet = container.AttributeSets[1]; // somehow COM starts a 1 not at zero

            var containerAttribute = containerAttributeSet[id];
            (containerAttribute.Value as string).Should().Be(value);
        }

        [TestMethod]
        public void property_exists_save_should_update_property()
        {
            //arange

            string value = Guid.NewGuid().ToString();
            string value2 = Guid.NewGuid().ToString();
            string id = "12345";

            var setsContainer = new AttributeSetsContainer();

            var attsetsMock = MockFactory.CreateAttributeSets(setsContainer);

            var setContainer = new AttributeSetContainer();
            setContainer.Name = "one";
            var attributeSet = MockFactory.CreateAttributeSet(setContainer, attsetsMock.Object).Object;
            setsContainer.AttributeSets.Add(attributeSet);
            setContainer.Attributes.Add(MockFactory.CreateAttribute(id, ValueTypeEnum.kStringType, value, attributeSet));
            var applicationMock = new Mock<IApplicationService>();

            applicationMock.Setup(x => x.GetCurrentDocumentAttributeSets()).Returns(attsetsMock.Object);

            var businessClass = new MyBusinessClass(applicationMock.Object);

            //act

            businessClass.SaveMyProperty(value2, id);

            //assert

            setContainer.Attributes.Count.Should().Be(1);

            (setContainer.Attributes[1].Value as string).Should().Be(value2);
        }
    }
}