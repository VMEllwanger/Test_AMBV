using Bogus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit;

public abstract class TestBase
{
  protected readonly Faker Faker;

  protected TestBase()
  {
    Faker = new Faker("pt_BR");
  }
}
