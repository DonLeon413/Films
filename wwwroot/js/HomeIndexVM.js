function HomeIndexVM() {

    var _self = this;


    // modal edit film
    _self.Name = ko.observable( '' )
    _self.Year = ko.observable( 0 );
    _self.Description = ko.observable( '' );
    _self.Producer = ko.observable( '' );
    _self.Image = ko.observable( '' );

    _self.Id = 0;

    _self.IsEditable = ko.observable( true );

    /////////////////////////////////////////////////////////////////////////////
    _self.EditContextCell = function( data, type, full, meta ) {
                
        var img_url = '/img/editar.png';

        var content_html = '<button class="btn btn-film-edit" type="button">' +
                '<img src="' + img_url + '" width="32px" height="32px" alt="lorem">'
        '</button> ';
        
        
        return content_html;
    }

    /////////////////////////////////////////////////////////////////////////////
    _self.DeleteContextCell = function( data, type, full, meta ) {

        var content_html = "";

        if ( false === full.is_read_only ) {
            var img_url = '/img/eliminar.png';

           content_html = '<button class="btn btn-film-delete" type="button">' +
                '<img src="' + img_url + '" width="32px" height="32px" alt="lorem">'
            '</button> ';
        }

        return content_html;
    }

//////////////////////////////////////////////////////////////////////////////////
    _self.Table = $( '#Films' ).DataTable( {
        "serverSide": true,
        "searching": true,
        "destroy": true,
        "processing": true,     // Show processing
        "stateSave": true,      // Save state
        "stateDuration": -1,    // Save state in session
        "ajax": {
            'url': 'Home/Films',
            'type': 'POST',
            'dataFilter': function( response ) {

                return response;
            }
        },
       // "language": DTLoadSpainLanguage( _self.UrlPrefix ),
        "columns": [
            {
                'title': 'Name', 'data': 'name',
                'searchEnable': true,
                'orderable': true
            },
            {
                'title': 'Year', 'data': 'year',
                'searchEnable': true,
                'orderable': true
            },
            {
                "title": "Produser",
                'data': 'produser',
                "searchable": true, 'orderable': true
            },
            {
                "title": "EDIT",
                'render': _self.EditContextCell,
                "searchable": false, 'orderable': false
            },
            {
                "title": "DELETE",
                'render': _self.DeleteContextCell,
                "searchable": false, 'orderable': false
            }
        ],
        "order": [[0, 'asc']],
        "lengthMenu": [ [5, 10], [5, 10]]
    } );

    
    $( '#Films tbody' ).on( 'click', '.btn-film-edit', function( e ) {
        var row_idx = $( this ).parents( 'tr' );
        _self.OnEdit( row_idx );
    } );

    $( '#Films tbody' ).on( 'click', '.btn-film-delete', function( e ) {
        var row_idx = $( this ).parents( 'tr' );
        _self.OnDelete( row_idx );
    } );

///////////////////////////////////////////////////////////////////////////////////
    _self.readURL = function( input ) {
        if ( input.files && input.files[0] ) {
            var reader = new FileReader();

            reader.onload = function( e ) {
                $( '#posterImage' ).attr( 'src', e.target.result );
            }

            reader.readAsDataURL( input.files[0] );
        }
    }

///////////////////////////////////////////////////////////////////////////////////
    $( "#poster" ).change( function() {
        _self.readURL( this );
    } );

/////////////////////////////////////////////////////////////////////////////
    _self.SaveFilm = function() {


        var formdata = new FormData();
        var fileInput = document.getElementById( 'poster' );

        if ( fileInput.files.length > 0 ) {
            formdata.append( 'file', fileInput.files[0] )
        }

        var convert = function( value ) {
            return ( value === null || value === undefined ? "" : value );
        }

        formdata.append( 'name', convert( _self.Name() ) );
        formdata.append( 'description', convert( _self.Description() ) );
        formdata.append( 'year', _self.Year() );
        formdata.append( 'producer', convert( _self.Producer() ) );
        formdata.append( 'id', _self.Id );

        var ajax_url = '/Home/SaveFilm';

        $.ajax( {
            url: ajax_url,
            data: formdata,
            type: 'POST',
            processData: false,
            contentType: false, 
            success: function( data ) {

                _self.Table.ajax.reload( null, false );
                $( '#EditFilmDlg' ).modal( 'hide' );

            },
            error: function( resultError ) {

                var mvcError = resultError.responseText;

                swal( {
                    "title": "Upsate film information error",
                    "text": mvcError,
                    "type": "error",
                    "confirmButtonClass": "btn btn-secondary m-btn m-btn--wide"
                } );

                return false;
            }
        } );
    }

/////////////////////////////////////////////////////////////////////////////
    _self.DoCreateFilm = function() {

        _self.DoEditFile( 0 );

    }

/////////////////////////////////////////////////////////////////////////////
    _self.OnEdit = function( rowIdx ) {

        var data_row = _self.Table.rows( rowIdx ).data();
        _self.DoEditFile( data_row[0].id );

    }

/////////////////////////////////////////////////////////////////////////////
    _self.DoEditFile = function( id ) {

        var ajax_url = '/Home/Film';

        $.ajax( {
            url: ajax_url,
            data: { id: id },
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function( data ) {

                _self.Name( data.name );
                _self.Description( data.description );
                _self.Year( data.year );
                _self.Producer( data.producer );
                _self.Image( data.file_name );
                _self.Id = data.id;

                _self.IsEditable( false === data.is_read_only );

                $( '#EditFilmDlg' ).modal( 'show' );

                _self.Table.ajax.reload( null, true );
            },
            error: function( resultError ) {

                var mvcError = resultError.responseText;

                swal( {
                    "title": "Load film information error",
                    "text": mvcError,
                    "type": "error",
                    "confirmButtonClass": "btn btn-secondary m-btn m-btn--wide"
                } );

                return false;
            }
        } );
    };

/////////////////////////////////////////////////////////////////////////////
    _self.OnDelete = function( rowIdx ) {

        var data_row = _self.Table.rows( rowIdx ).data();
        swal( {
            title: "Are you sure?",
            text: "Delete film?",
            icon: "warning",
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            showCancelButton: true,
            showCloseButton: true,
        } ).then( ( result ) => {
            if ( result.value ) {

                var ajax_url = '/Home/FilmDelete';

                $.ajax( {
                    url: ajax_url,
                    data: JSON.stringify( data_row[0].id ),
                    type: 'DELETE',
                    //dataType: 'JSON',
                    contentType: 'application/json; charset=utf-8',
                    success: function( data ) {

                        _self.Table.ajax.reload( null, true );

                    },
                    error: function( resultError ) {

                        var mvcError = resultError.responseText;

                        swal( {
                            "title": "Delete error",
                            "text": mvcError,
                            "type": "error",
                            "confirmButtonClass": "btn btn-secondary m-btn m-btn--wide"
                        } );

                        return false;
                    }
                } );

            }
        } );
    }

}