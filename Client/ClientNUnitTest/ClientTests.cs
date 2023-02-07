namespace ClientNUnitTest
{
    public class Test
    {
        [SetUp]
        public void Setup()
        {
            //I setup kan vi ist�llet instansiera v�rat MathClass math variabel
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