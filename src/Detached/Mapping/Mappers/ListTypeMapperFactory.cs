﻿using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers
{
    public class ListMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsCollection
                && typeMap.TargetOptions.IsCollection
                && !typeMap.ItemMap.TargetOptions.IsEntity;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.Source),
                        Parameter(typeMap.Target),
                        Parameter(typeMap.Context),
                        Block(
                            CreateMapper(typeMap.ItemMap),
                            If(IsNull(typeMap.Source),
                                Then(
                                    Assign(typeMap.Target, Default(typeMap.Target.Type))
                                ),
                                Else(
                                    Assign(typeMap.Target, typeMap.TargetOptions.Construct(typeMap.Context)),
                                    Variable(typeMap.ItemMap.Source),
                                    ForEach(
                                        typeMap.ItemMap.Source,
                                        In(typeMap.Source),
                                        Block(
                                            Call("Add",
                                                typeMap.Target,
                                                CallMapper(
                                                    typeMap.ItemMap,
                                                    typeMap.ItemMap.Source,
                                                    Default(typeMap.ItemMap.Target.Type)
                                                )
                                            )
                                        )
                                    )
                                )
                            ),
                            Result(typeMap.Target)
                        )
                    );
        }
    }
}