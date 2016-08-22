// Get a standard NetSuite record
/**
 * @param {Object} dataIn Parameter object
 * @returns {Object} Output object
 */
function getRecord(dataIn) {
    return nlapiLoadRecord(dataIn.type, dataIn.id); // e.g type="customer", id="769"
}

// Delete a standard NetSuite record
/**
 * @param {Object} dataIn Parameter object
 * @returns {Void}
 */
function delRecord(dataIn) {
    nlapiDeleteRecord(dataIn.type, dataIn.id); // e.g type="customer", id="769"
}

// Create a standard NetSuite record via POST method
/**
 * @param {Object} dataIn Parameter object
 * @returns {Object} Output object
 */
function postRecord(dataIn) {
    var err = new Object();

    // Validate if mandatory record type is set in the request
    if (!dataIn.type) {
        err.status = "failed";
        err.message = "missing type";
        return err;
    }
    var request_body = dataIn['request_body'];
    var record = nlapiCreateRecord(dataIn.type, { recordmode: 'dynamic' }); // added , {recordmode: 'dynamic'}

    for (var fieldname in request_body) {
        if (request_body.hasOwnProperty(fieldname)) {
            var value = request_body[fieldname];
            nlapiLogExecution('DEBUG', 'fieldname=' + fieldname, 'value=' + value + ' typeof=' + typeof value);
            if (value && typeof value != 'object') // ignore other type of parameters
            {
                record.setFieldValue(fieldname, value);
            }
                // address sub record logic
            else if (fieldname == 'addressbook') {
                for (var fieldname2 in value) {
                    var value2 = value[fieldname2];
                    nlapiLogExecution('DEBUG', '**fieldname=' + fieldname2, '**value2=' + value2 + ' typeof=' + typeof value2);
                    //Add first line to sublist
                    record.selectNewLineItem('addressbook');
                    if (value2 && typeof value2 != 'object') {
                        //record.setCurrentLineItemValue('addressbook', 'fieldname2', value2);
                    } else {
                        //var subrecord = record.createCurrentLineItemSubrecord('addressbook', 'addressbookaddress');
                        for (var fieldname3 in value2) {
                            var value3 = value2[fieldname3];
                            nlapiLogExecution('DEBUG', '@fieldname=' + fieldname3, '**value3=' + value3 + ' typeof=' + typeof value3);

                            if (value3 && typeof value3 != 'object') {
                                subrecord.setFieldValue(fieldname3, value3);
                            }
                        }
                        subrecord.commit();
                        record.commitLineItem(fieldname);
                    }
                }
            }
            else if (fieldname == 'member') {
                for (var fieldname2 in value) {
                    var value2 = value[fieldname2];
                    nlapiLogExecution('DEBUG', '**fieldname=' + fieldname2, '**value2=' + value2 + ' typeof=' + typeof value2);

                    if (value2 && typeof value2 != 'object') {
                        record.setFieldValue(fieldname2, value2);
                    } else {
                        record.selectNewLineItem(fieldname);

                        for (var fieldname3 in value2) {
                            var value3 = value2[fieldname3];
                            nlapiLogExecution('DEBUG', '@fieldname=' + fieldname3, '**value3=' + value3 + ' typeof=' + typeof value3);

                            if (value3 && typeof value3 != 'object') {
                                record.setCurrentLineItemValue(fieldname, fieldname3, value3);
                            }
                        }
                        record.commitLineItem(fieldname);
                    }
                }
            }
        }
    }
    var id = nlapiSubmitRecord(record);
    nlapiLogExecution('DEBUG', 'id=' + id);

    var nlobj = nlapiLoadRecord(dataIn.type, id);
    return nlobj;
}

//Updates a standard NetSuite record via PUT method
/**
 * @param {Object} dataIn Parameter object
 * @returns {Object} Output object
 */
function putRecord(dataIn) {
    var err = new Object();

    // Validate if mandatory record type is set in the request
    if (!dataIn.type) {
        err.status = "failed";
        err.message = "missing type";
        return err;
    }

    var record = nlapiLoadRecord(dataIn.type, dataIn.id);
    var request_body = dataIn['request_body'];

    for (var fieldname in request_body) {
        if (request_body.hasOwnProperty(fieldname)) {
            var value = request_body[fieldname];
            nlapiLogExecution('DEBUG', 'fieldname=' + fieldname, 'value=' + value + ' typeof=' + typeof value);
            if (value && typeof value != 'object') // ignore other type of parameters
            {
                record.setFieldValue(fieldname, value);
            } else if (fieldname == 'member') {
                for (var fieldname2 in value) {
                    var value2 = value[fieldname2];
                    nlapiLogExecution('DEBUG', '**fieldname=' + fieldname2, '**value2=' + value2 + ' typeof=' + typeof value2);

                    if (value2 && typeof value2 != 'object') {
                        record.setFieldValue(fieldname2, value2);
                    } else {
                        record.selectNewLineItem(fieldname);

                        for (var fieldname3 in value2) {
                            var value3 = value2[fieldname3];
                            nlapiLogExecution('DEBUG', '@fieldname=' + fieldname3, '**value3=' + value3 + ' typeof=' + typeof value3);

                            if (value3 && typeof value3 != 'object') {
                                record.setCurrentLineItemValue(fieldname, fieldname3, value3);
                            }
                        }
                        record.commitLineItem(fieldname);
                    }
                }
            }
        }
    }
    var id = nlapiSubmitRecord(record);
    nlapiLogExecution('DEBUG', 'id=' + id);

    var nlobj = nlapiLoadRecord(dataIn.type, id);
    return nlobj;
}