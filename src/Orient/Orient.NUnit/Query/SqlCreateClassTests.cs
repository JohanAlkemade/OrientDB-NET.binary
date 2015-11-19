﻿using System;
using NUnit.Framework;
using Orient.Client;
using System.Linq;

namespace Orient.Tests.Query
{
    [TestFixture]
    public class SqlCreateClassTests
    {
        [Test]
        public void ShouldCreateClass()
        {
            using (TestDatabaseContext testContext = new TestDatabaseContext())
            {
                using (ODatabase database = new ODatabase(TestConnection.GlobalTestDatabaseAlias))
                {
                    short classId1 = database
                        .Create.Class("TestClass1")
                        .Run();

                    Assert.IsTrue(classId1 > 0);

                    short classId2 = database
                        .Create.Class("TestClass2")
                        .Run();

                    Assert.IsTrue(classId2 > 0);

                    Assert.AreEqual(classId1 + 1, classId2);
                }
            }
        }

        [Test]
        public void ShouldCreateClassExtends()
        {
            using (TestDatabaseContext testContext = new TestDatabaseContext())
            {
                using (ODatabase database = new ODatabase(TestConnection.GlobalTestDatabaseAlias))
                {
                    short classId1 = database
                        .Create.Class("TestClass1")
                        .Extends("OVertex")
                        .Run();

                    Assert.IsTrue(classId1 > 0);

                    short classId2 = database
                        .Create.Class("TestClass2")
                        .Extends<OVertex>()
                        .Run();

                    Assert.IsTrue(classId2 > 0);

                    Assert.AreEqual(classId1 + 1, classId2);
                }
            }
        }

        [Test]
        public void ShouldCreateAbstractClass()
        {
            using (TestDatabaseContext testContext = new TestDatabaseContext())
            {
                using (ODatabase database = new ODatabase(TestConnection.GlobalTestDatabaseAlias))
                {
                    short classId1 = database
                        .Create.Class("TestClass1")
                        .Abstract()
                        .Run();

                    Assert.IsTrue(classId1 > 0);

                    short classId2 = database
                        .Create.Class("TestClass2")
                        .Abstract()
                        .Run();

                    Assert.IsTrue(classId2 > 0);

                    Assert.AreEqual(classId1 + 1, classId2);
                }
            }
        }

        [Test]
        public void ShouldCreateClassCluster()
        {
            using (TestDatabaseContext testContext = new TestDatabaseContext())
            {
                using (ODatabase database = new ODatabase(TestConnection.GlobalTestDatabaseAlias))
                {
                    short clusterid1 = database
                        .Create
                        .Cluster("ClasterForTest1", OClusterType.None)
                        .Run();

                    short clusterid2 = database
                        .Create
                        .Cluster("ClasterForTest2", OClusterType.None)
                        .Run();

                    short classId1 = database
                        .Create.Class("TestClass1")
                        .Cluster(clusterid1)
                        .Run();

                    Assert.IsTrue(classId1 > 0);

                    short classId2 = database
                        .Create.Class("TestClass2")
                        .Cluster(clusterid2)
                        .Run();

                    Assert.AreEqual(classId1 + 1, classId2);
                }
            }
        }

        class TestClass
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public bool Flag { get; set; }
            public long LongValue { get; set; }
            public short ShortValue { get; set; }
            public byte Byte { get; set; }
            public byte[] data { get; set; }
        }

        [Test]
        public void ShouldCreateClassWithProperties()
        {
            using (TestDatabaseContext testContext = new TestDatabaseContext())
            {
                using (ODatabase database = new ODatabase(TestConnection.GlobalTestDatabaseAlias))
                {
                    short classId1 = database.Create.Class<TestClass>().CreateProperties().Run();

                    Assert.IsTrue(classId1 > 0);

                    // would like to test the properties have been created properly, but the server doesn't implement 'info' over the remove protocol
                    //var result = database.Command("info class TestClass");
                    
                    var schema = database.Schema.Properties<TestClass>();
                }
            }
        }

        [Test]
        public void ShouldCreateClassWithCustomNameProperties()
        {
            using (TestDatabaseContext testContext = new TestDatabaseContext())
            {
                using (ODatabase database = new ODatabase(TestConnection.GlobalTestDatabaseAlias))
                {
                    short classId1 = database.Create.Class<TestClass>("MyTestClass").CreateProperties().Run();

                    Assert.IsTrue(classId1 > 0);

                    
                }
            }
        }
    }
}
