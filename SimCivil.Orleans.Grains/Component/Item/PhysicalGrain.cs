using Microsoft.Extensions.Logging;
using SimCivil.Orleans.Grains.Strategy;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class PhysicalGrain : BaseGrain<PhysicalState>, IPhysical
    {
        public const string NotSinglePart = "The part is not a single part.";
        public const string NotAssemblyPart = "The part is not an assembly part.";

        public PhysicalGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        public async Task<Result> AddCompounds(IDictionary<string, double> compounds)
        {
            if (State.Part is null)
            {
                State.Part = new SinglePart();
            }

            if (State.Part is SinglePart singlePart)
            {
                foreach (var compound in compounds)
                {
                    singlePart.CompoundWeights[compound.Key] =
                        (singlePart.CompoundWeights.TryGetValue(compound.Key, out double val) ? val : 0) + compound.Value;
                }
            }
            else
            {
                return new Result(ErrorCode.InvalidOperation, NotSinglePart);
            }

            await WriteStateAsync();
            return new Result();
        }

        public async Task<Result> AddPhysicalPart(IDictionary<string, IPhysicalPart> subparts)
        {
            if (State.Part is null)
            {
                State.Part = new AssemblyPart();
            }

            var conflicts = new List<string>();
            if (State.Part is AssemblyPart assemblyPart)
            {
                foreach (var part in subparts)
                {
                    if (assemblyPart.Parts.TryGetValue(part.Key, out IPhysicalPart val))
                    {
                        conflicts.Add(part.Key);
                        continue;
                    }

                    assemblyPart.Parts[part.Key] = part.Value;
                }
            }
            else
            {
                return new Result(ErrorCode.InvalidOperation, NotAssemblyPart);
            }

            await WriteStateAsync();
            var ret = new Result();
            if (conflicts.Count > 0)
            {
                ret.Err = ErrorCode.PartiallyComplete;
                ret.ErrMsg = $"Parts already exist: [{string.Join(','.ToString(), conflicts)}]";
            }
            return new Result();
        }

        public async Task Clear()
        {
            State.Part = null;
            await WriteStateAsync();
        }

        public Task<Result<IDictionary<string, double>>> GetCompounds()
        {
            if (State.Part is SinglePart singlePart)
            {
                return Task.FromResult(new Result<IDictionary<string, double>>(singlePart.CompoundWeights));
            }
            else if (State.Part is null)
            {
                return Task.FromResult(new Result<IDictionary<string, double>>(new Dictionary<string, double>()));
            }

            return Task.FromResult(new Result<IDictionary<string, double>>(ErrorCode.InvalidOperation, NotSinglePart));
        }

        public Task<Result<IDictionary<string, IPhysicalPart>>> GetPhysicalParts()
        {
            if (State.Part is AssemblyPart part)
            {
                return Task.FromResult(new Result<IDictionary<string, IPhysicalPart>>(part.Parts));
            }
            else if (State.Part is null)
            {
                return Task.FromResult(new Result<IDictionary<string, IPhysicalPart>>(new Dictionary<string, IPhysicalPart>()));
            }

            return Task.FromResult(new Result<IDictionary<string, IPhysicalPart>>(ErrorCode.InvalidOperation, NotAssemblyPart));
        }

        public Task<double> GetVolume()
        {
            return Task.FromResult(State.Part.Volume);
        }

        public Task<double> GetWeight()
        {
            return Task.FromResult(State.Part.Weight);
        }

        public Task<bool> IsAssembly()
        {
            return Task.FromResult(State.Part is AssemblyPart || State.Part is null);
        }

        public Task<bool> IsSinglePart()
        {
            return Task.FromResult(State.Part is SinglePart || State.Part is null);
        }

        public Task<Result> RemoveCompound(IEnumerable<string> compounds)
        {
            var conflicts = new List<string>();
            if (State.Part is SinglePart part)
            {
                foreach (var comp in compounds)
                {
                    if (part.CompoundWeights.ContainsKey(comp))
                    {
                        part.CompoundWeights.Remove(comp);
                        continue;
                    }

                    conflicts.Add(comp);
                }
            }
            else if (State.Part is null)
            {
                if (compounds.Any())
                {
                    return Task.FromResult(new Result(ErrorCode.PartiallyComplete, "The part is empty."));
                }

                return Task.FromResult(new Result());
            }
            else
            {
                return Task.FromResult(new Result(ErrorCode.InvalidOperation, NotSinglePart));
            }

            if (conflicts.Count > 0)
            {
                return Task.FromResult(new Result(ErrorCode.PartiallyComplete, $"Cannot find components: [{string.Join(','.ToString(), conflicts)}]"));
            }

            return Task.FromResult(new Result());
        }

        public Task<Result> RemovePhysicalPart(IEnumerable<string> partNames)
        {
            var conflicts = new List<string>();
            if (State.Part is AssemblyPart part)
            {
                foreach (var name in partNames)
                {
                    if (part.Parts.ContainsKey(name))
                    {
                        part.Parts.Remove(name);
                        continue;
                    }

                    conflicts.Add(name);
                }
            }
            else if (State.Part is null)
            {
                if (partNames.Any())
                {
                    return Task.FromResult(new Result(ErrorCode.PartiallyComplete, "The part is empty."));
                }

                return Task.FromResult(new Result());
            }
            else
            {
                return Task.FromResult(new Result(ErrorCode.InvalidOperation, NotAssemblyPart));
            }

            if (conflicts.Count > 0)
            {
                return Task.FromResult(new Result(ErrorCode.PartiallyComplete, $"Cannot find parts: [{string.Join(','.ToString(), conflicts)}]"));
            }

            return Task.FromResult(new Result());
        }

        public async Task<Result> SetCompounds(IDictionary<string, double> compounds)
        {
            if (State.Part is null || State.Part is SinglePart)
            {
                State.Part = new SinglePart()
                {
                    CompoundWeights = compounds
                };

                await WriteStateAsync();
                return new Result();
            }

            return new Result(ErrorCode.InvalidOperation, NotSinglePart);
        }

        public async Task<Result> SetPhysicalParts(IDictionary<string, IPhysicalPart> subparts)
        {
            if (State.Part is null || State.Part is AssemblyPart)
            {
                State.Part = new AssemblyPart()
                {
                    Parts = subparts
                };

                await WriteStateAsync();
                return new Result();
            }

            return new Result(ErrorCode.InvalidOperation, NotAssemblyPart);
        }

        public Task<Result<AssemblyPart>> ToAssemblyPart()
        {
            if (State.Part is AssemblyPart part)
            {
                return Task.FromResult(new Result<AssemblyPart>(part));
            }
            else if (State.Part is null)
            {
                return Task.FromResult(new Result<AssemblyPart>(new AssemblyPart()));
            }

            return Task.FromResult(new Result<AssemblyPart>(ErrorCode.InvalidOperation, NotAssemblyPart));
        }

        public Task<Result<SinglePart>> ToSinglePart()
        {
            if (State.Part is SinglePart part)
            {
                return Task.FromResult(new Result<SinglePart>(part));
            }
            else if (State.Part is null)
            {
                return Task.FromResult(new Result<SinglePart>(new SinglePart()));
            }

            return Task.FromResult(new Result<SinglePart>(ErrorCode.InvalidOperation, NotSinglePart));
        }

        public async Task<Result> UpdateCompounds(IDictionary<string, double> compounds)
        {
            if (State.Part is null)
            {
                State.Part = new SinglePart();
            }

            if (State.Part is SinglePart part)
            {
                foreach (var comp in compounds)
                {
                    if (comp.Value == 0)
                    {
                        part.CompoundWeights.Remove(comp.Key);
                    }
                    else
                    {
                        part.CompoundWeights[comp.Key] = comp.Value;
                    }
                }
            }
            else
            {
                return new Result(ErrorCode.InvalidOperation, NotSinglePart);
            }

            await WriteStateAsync();
            return new Result();
        }

        public async Task<Result> UpdatePhysicalPart(IDictionary<string, IPhysicalPart> subparts)
        {
            if (State.Part is null)
            {
                State.Part = new AssemblyPart();
            }

            if (State.Part is AssemblyPart part)
            {
                foreach (var subpart in subparts)
                {
                    if (subpart.Value is null)
                    {
                        part.Parts.Remove(subpart.Key);
                    }
                    else
                    {
                        part.Parts[subpart.Key] = subpart.Value;
                    }
                }
            }
            else
            {
                return new Result(ErrorCode.InvalidOperation, NotAssemblyPart);
            }

            await WriteStateAsync();
            return new Result();
        }
    }
}
