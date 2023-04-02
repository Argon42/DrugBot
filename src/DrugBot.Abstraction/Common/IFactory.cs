namespace DrugBot.Core.Common;

public interface IFactory<out T>
{
    T Create();
}

public interface IFactory<out T, in TArg1>
{
    T Create(TArg1 arg1);
}

public interface IFactory<out T, in TArg1, in TArg2>
{
    T Create(TArg1 arg1, TArg2 arg2);
}