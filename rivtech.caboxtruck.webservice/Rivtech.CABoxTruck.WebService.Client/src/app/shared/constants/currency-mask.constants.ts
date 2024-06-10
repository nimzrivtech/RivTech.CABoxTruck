const _noNegativeSetMax = (maxInput: number, suffix?: string) => {
    return {
        align: "left",
        allowNegative: false,
        max: maxInput,
        prefix: '',
        suffix: suffix ? suffix : '',
        precision: 0,
        nullable: true
    };
}

const _monetarySetMax = (maxInput: number) => {
    return {
        align: "left",
        allowNegative: false,
        decimal: ",",
        max: maxInput,
        prefix: '$ ',
        precision: 0,
        nullable: true
    };
}

export const CurrencyMaskConstants = {    
    HourPerDay: _noNegativeSetMax(24),
    HourPerWeek: _noNegativeSetMax(168),
    Percentage: _noNegativeSetMax(100, '%'), 
    Year: _noNegativeSetMax(999),    
    MonetaryMax: _monetarySetMax(9999999999), 
    NonMonetaryMax: _noNegativeSetMax(999999), 

    setMaxInput(maxInput: number): any {        
        return _noNegativeSetMax(maxInput);
    }
};