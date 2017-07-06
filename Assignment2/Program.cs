using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm;

namespace GeneticAlgorithm
{
    class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            Func<Individual> createIndividual = CreateIndividual;
            Func<Individual, double> computeFitness = ComputeFitness;
            Func<Individual[], double[], Func<Tuple<Individual, Individual>>> selectTwoParents = SelectTwoParents;
            Func<Tuple<Individual, Individual>, Tuple<Individual, Individual>> crossover = CrossOver;
            Func<Individual, double, Individual> mutation = Mutation;

            GeneticAlgorithm<Individual> fakeProblemGA = new GeneticAlgorithm<Individual>(0.85, 0.01, true, 10, 50); // CHANGE THE GENERIC TYPE (NOW IT'S INT AS AN EXAMPLE) AND THE PARAMETERS VALUES
            var solution = fakeProblemGA.Run(createIndividual, computeFitness, selectTwoParents, crossover, mutation);
            Console.WriteLine("Average fitness: " + solution.Item2.Item2);
            Console.WriteLine("Best fitness: " + solution.Item2.Item1);
            Console.WriteLine("Best individual: " + solution.Item1);
            Console.ReadLine();
        }

        static Individual Mutation(Individual ind, double mutationRate)
        {
            var mutated = ind.solution;
            for (int i = 0; i < 5; i++)
            {
                var nextRandom = rnd.NextDouble();

                if (nextRandom < mutationRate)
                    mutated ^= (1 << i);
            }
            return new Individual(mutated);
        }

        static Tuple<Individual, Individual> CrossOver(Tuple<Individual, Individual> individuals)
        {
            return individuals.Item1.Breed(individuals.Item2);
        }

        static Func<Tuple<Individual, Individual>> SelectTwoParents(Individual[] population, double[] fitnesses)
        {
            return () =>
            {
                var populationWithFitnessOrdered =
                    population.AsEnumerable()
                        .Select((ind, index) => new { Individual = ind, Fitness = fitnesses[index] })
                        .OrderBy(ind => ind.Fitness)
                        .Select((x, index) => new { Individual = x.Individual, Fitness = index + 1 }).ToList();

                var populationReplicated =
                    populationWithFitnessOrdered
                        .Select((x, index) => Enumerable.Range(0, x.Fitness).Select(i => Tuple.Create(index, x.Individual)))
                        .SelectMany(i => i).ToArray();

                var ind1 = populationReplicated[rnd.Next(0, populationReplicated.Count())];

                populationReplicated = populationReplicated.Where(x => x.Item1 != ind1.Item1).ToArray();

                var ind2 = populationReplicated[rnd.Next(0, populationReplicated.Count())];

                return Tuple.Create<Individual, Individual>(ind1.Item2, ind2.Item2);
            };
        } 

        static Individual CreateIndividual()
        {
            return new Individual(rnd.Next(0, 32));
        }

        static double ComputeFitness(Individual ind)
        {
            return (-Math.Pow((double)ind.solution, 2)) + (ind.solution * 7);
        }
    }
}