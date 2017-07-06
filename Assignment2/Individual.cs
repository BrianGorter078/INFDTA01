using System;

namespace GeneticAlgorithm
{
    public class Individual
    {
        Random random = new Random();
        public int solution = 0;
        public Individual(int amount)
        {
            if (amount <= 31 && amount >= 0)
                solution = amount;
        }

        public Tuple<Individual, Individual> Breed(Individual other)
        {

            var ownPattern = Convert.ToByte(solution);
            var otherPattern = Convert.ToByte(other.solution);

            var binaryPatternOwn = Convert.ToString(ownPattern, 2).PadLeft(5, '0');
            var binaryPatternOther = Convert.ToString(otherPattern, 2).PadLeft(5, '0');

            var index = random.Next(0, 5);

            var child = binaryPatternOwn.Substring(0, index) + binaryPatternOther.Substring(index);
            var offspring = binaryPatternOther.Substring(0, index) + binaryPatternOwn.Substring(index);

            return Tuple.Create<Individual, Individual>(new Individual(Convert.ToInt32(child, 2)), new Individual(Convert.ToInt32(offspring, 2)));
        }

        public override string ToString()
        {
            return solution.ToString();
        }
    }
}