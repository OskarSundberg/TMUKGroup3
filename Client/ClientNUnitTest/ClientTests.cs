namespace ClientNUnitTest
{
    public class Test
    {
        [SetUp]
        public void Setup()
        {
            //I setup kan vi istället instansiera vårat MathClass math variabel
        }

        [Test]
        public void TestClientName()
        {
            //ARRANGE
            Client client = new Client();

            //ACT
            client.Name = "Test";

            //ASSERT
            Assert.That(client.Name, Is.EqualTo("Test"));

        }
    }
}