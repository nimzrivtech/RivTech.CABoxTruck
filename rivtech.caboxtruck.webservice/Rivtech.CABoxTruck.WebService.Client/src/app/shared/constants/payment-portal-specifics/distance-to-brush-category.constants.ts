const DistanceToBrushCategory = (min, max?) => {
    return {
        min: min,
        max: max || 9999,
        toString: () => {
                return max?  `${min}-${max}` : `${min}`;
        }
    };
}

export const DistanceToBrushCategoryConstants = {
    ZeroToThirty: DistanceToBrushCategory(0, 30),
    ThirtyOneToFifty: DistanceToBrushCategory(31, 50),
    FiftyOneToOneHundred: DistanceToBrushCategory(51, 100),
    OneHundredOneToTwoHundred: DistanceToBrushCategory(101, 200),
    TwoHundredOneToThreeHundred: DistanceToBrushCategory(201, 300),
    ThreeHundredOneToFourHundred: DistanceToBrushCategory(301, 400),
    FourHundredOneToSixHundred: DistanceToBrushCategory(401, 600),
    SixHundredOneToOneThousand: DistanceToBrushCategory(601, 1000),
    OneThousandOneToOneThousandTwoHundredFifty: DistanceToBrushCategory(1000, 1250),
    OneThousandTwoHundredFifty: DistanceToBrushCategory(1250),
}